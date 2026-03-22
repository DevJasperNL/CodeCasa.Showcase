using CodeCasa.Mqtt.BackgroundService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;

namespace CodeCasa.Mqtt.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeCasaMqtt(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<MqttOptions>(configuration.GetSection("MqttOptions"));

        serviceCollection
            .AddSingleton(new MqttClientFactory().CreateMqttClient())
            .AddSingleton<MqttPublisher>();
        serviceCollection.AddHostedService<MqttWorker>();

        return serviceCollection;
    }
}