using System.Reactive;
using System.Reactive.Linq;
using CodeCasa.CustomEntities.Core.Extensions;
using NetDaemon.Devices.Abstractions;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Core.Switches
{
    public abstract class HueDimmerSwitch : IDimmer
    {
        // todo: have a look in zigbee2mqtt if this is all
        private const string OnPress = "on_press";
        private const string OffPress = "off_press"; // scene button
        // Dimming
        private const string DownPress = "down_press";
        private const string DownPressRelease = "down_press_release";
        private const string DownHoldRelease = "down_hold_release";
        // Brightening
        private const string UpPress = "up_press";
        private const string UpPressRelease = "up_press_release";
        private const string UpHoldRelease = "up_hold_release";

        protected HueDimmerSwitch(ITriggerManager triggerManager, string mqttDeviceName)
        {
            OnOffPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == OnPress)
                .Select(_ => Unit.Default);
            ScenePressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == OffPress)
                .Select(_ => Unit.Default);
            var dimStart = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == DownPress)
                .Select(_ => true);
            var dimStop = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x is DownPressRelease or DownHoldRelease)
                .Select(_ => false);
            Dimming = dimStart.Merge(dimStop).DistinctUntilChanged().Prepend(false);
            var brightenStart = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == UpPress)
                .Select(_ => true);
            var brightenStop = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x is UpPressRelease or UpHoldRelease)
                .Select(_ => false);
            Brightening = brightenStart.Merge(brightenStop).DistinctUntilChanged().Prepend(false);
        }

        public IObservable<Unit> OnOffPressed { get; }
        public IObservable<Unit> ScenePressed { get; }
        public IObservable<bool> Dimming { get; }
        public IObservable<bool> Brightening { get; }
    }
}
