using Microsoft.Extensions.Options;
using MQTTnet;

namespace CodeCasa.Mqtt
{
    public class MqttPublisher(IMqttClient mqttClient, IOptions<MqttOptions> options)
    {
        private readonly MqttOptions _options = options.Value;

        public async Task<MqttClientPublishResult?> PublishAsync(string entityId, string message)
        {
            if (!mqttClient.IsConnected)
            {
                return null;
            }

            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"{_options.BaseTopic}/{entityId}")
                .WithPayload(message)
                .Build();

            return await mqttClient.PublishAsync(mqttMessage);
        }
    }
}
