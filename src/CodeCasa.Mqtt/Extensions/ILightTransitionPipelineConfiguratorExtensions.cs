using System.Reactive;
using System.Reactive.Linq;
using CodeCasa.AutomationPipelines;
using CodeCasa.AutomationPipelines.Lights.Pipeline;
using CodeCasa.Lights;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CodeCasa.Mqtt.Extensions
{
    public static class ILightTransitionPipelineConfiguratorExtensions
    {
        public static ILightTransitionPipelineConfigurator<TLight> PublishTelemetryToMqtt<TLight>(this ILightTransitionPipelineConfigurator<TLight> configurator, MqttPublisher publisher) where TLight : ILight
        {
            configurator.OnCompleted(e =>
            {
                PublishPipelineState(publisher, e.Pipeline, e.Light.Id).GetAwaiter().GetResult();
            });
            configurator.ConfigureTelemetrySubscriber(stream =>
            {
                return stream.SelectMany(async t =>
                {
                    await PublishPipelineState(publisher, t.Pipeline, t.Light.Id);
                    return Unit.Default;
                }).Subscribe();
            });
            return configurator;
        }

        private static async Task PublishPipelineState(MqttPublisher publisher, IPipeline<LightTransition> pipeline, string lightEntityId)
        {
            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new IgnoreObservablesContractResolver(), ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            var jsonString = JsonConvert.SerializeObject(pipeline, settings);
            await publisher.PublishAsync(lightEntityId, jsonString);
        }

        private class IgnoreObservablesContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                if (property.PropertyType is not null &&
                    property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(IObservable<>))
                {
                    property.ShouldSerialize = _ => false;
                }

                return property;
            }
        }
    }
}
