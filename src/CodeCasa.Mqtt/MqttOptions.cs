
namespace CodeCasa.Mqtt
{
    public class MqttOptions
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; } = 1883;
        public string? User { get; set; }
        public string? Password { get; set; }
        public string BaseTopic { get; set; } = "code-casa";
    }
}
