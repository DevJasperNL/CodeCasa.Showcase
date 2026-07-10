using System.Reactive;
using System.Reactive.Linq;
using CodeCasa.Abstractions;
using CodeCasa.CustomEntities.Core.Extensions;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Core.Switches
{
    public abstract class IkeaRodretDimmer : IDimmer
    {
        private const string On = "on";
        private const string Off = "off";
        private const string BrightnessMoveUp = "brightness_move_up";
        private const string BrightnessMoveDown = "brightness_move_down";
        private const string BrightnessStop = "brightness_stop";

        protected IkeaRodretDimmer(ITriggerManager triggerManager, string mqttDeviceName)
        {
            OnPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == On)
                .Select(_ => Unit.Default);
            OffPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == Off)
                .Select(_ => Unit.Default);
            var dimStart = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == BrightnessMoveDown)
                .Select(_ => true);
            var brightenStart = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == BrightnessMoveUp)
                .Select(_ => true);
            var dimOrbrightenStop = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == BrightnessStop)
                .Select(_ => false);
            Dimming = dimStart.Merge(dimOrbrightenStop).DistinctUntilChanged().Prepend(false);
            Brightening = brightenStart.Merge(dimOrbrightenStop).DistinctUntilChanged().Prepend(false);
        }

        public IObservable<Unit> OnPressed { get; }
        public IObservable<Unit> OffPressed { get; }
        public IObservable<bool> Dimming { get; }
        public IObservable<bool> Brightening { get; }
    }
}
