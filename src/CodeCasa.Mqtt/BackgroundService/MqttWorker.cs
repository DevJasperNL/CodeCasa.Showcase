using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Formatter;

namespace CodeCasa.Mqtt.BackgroundService
{
    public class MqttWorker(IMqttClient client, IOptions<MqttOptions> options)
        : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly MqttOptions _options = options.Value;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionBuilder = new MqttClientOptionsBuilder()
                .WithTcpServer(_options.Host, _options.Port)
                .WithProtocolVersion(MqttProtocolVersion.V500);
            if (_options.User != null)
            {
                connectionBuilder.WithCredentials(_options.User, _options.Password!);
            }
            var connectOptions = connectionBuilder.Build();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!client.IsConnected)
                    {
                        await client.ConnectAsync(connectOptions, stoppingToken);
                        Console.WriteLine("MQTT Connected.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}. Retrying...");
                }

                // Wait before checking connection status again
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
