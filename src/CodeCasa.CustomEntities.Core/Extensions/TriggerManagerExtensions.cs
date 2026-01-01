using System.Reactive.Linq;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Core.Extensions
{
    public static class TriggerManagerExtensions
    {
        public static IObservable<string> Zigbee2MqttActions(this ITriggerManager triggerManager, string mqttDeviceName)
        {
            var triggerTopic = triggerManager.RegisterTrigger(new
            {
                platform = "mqtt",
                topic = $"zigbee2mqtt/{mqttDeviceName}/action"
            });
            return triggerTopic.Select(e => e.GetProperty("payload").GetString()).Where(s => s != null)!;
        }
    }
}
