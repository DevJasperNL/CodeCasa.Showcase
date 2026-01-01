using System.Reactive;
using System.Reactive.Linq;
using CodeCasa.Abstractions;
using CodeCasa.CustomEntities.Core.Extensions;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Core.Switches
{
    public abstract class IkeaStyrbarRemoteControl : IDimmer
    {
        // Light large (top)
        private const string On = "on";
        private const string BrightnessMoveUp = "brightness_move_up";
        private const string BrightnessStop = "brightness_stop";

        // Light small (bottom)
        private const string Off = "off";
        private const string BrightnessMoveDown = "brightness_move_down";
        // BrightnessStop already defined above

        // Arrow left
        private const string ArrowLeftClick = "arrow_left_click";
        private const string ArrowLeftHold = "arrow_left_hold";
        private const string ArrowLeftRelease = "arrow_left_release";

        // Arrow right
        private const string ArrowRightClick = "arrow_right_click";
        private const string ArrowRightHold = "arrow_right_hold";
        private const string ArrowRightRelease = "arrow_right_release";

        protected IkeaStyrbarRemoteControl(ITriggerManager triggerManager, string mqttDeviceName)
        {
            LeftPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == ArrowLeftClick)
                .Select(_ => Unit.Default);
            RightPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == ArrowRightClick)
                .Select(_ => Unit.Default);

            SmallLightPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == Off)
                .Select(_ => Unit.Default);
            LargeLightPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == On)
                .Select(_ => Unit.Default);

            var leftHoldStart = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x is ArrowLeftClick or ArrowLeftHold)
                .Select(_ => true);
            var leftHoldStop = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == ArrowLeftRelease)
                .Select(_ => false);
            LeftHold = leftHoldStart.Merge(leftHoldStop).DistinctUntilChanged().Prepend(false);

            var rightHoldStart = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x is ArrowRightClick or ArrowRightHold)
                .Select(_ => true);
            var rightHoldStop = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == ArrowRightRelease)
                .Select(_ => false);
            RightHold = rightHoldStart.Merge(rightHoldStop).DistinctUntilChanged().Prepend(false);

            var dimStart = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x is Off or BrightnessMoveDown)
                .Select(_ => true);
            var dimStop = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == BrightnessStop)
                .Select(_ => false);
            Dimming = dimStart.Merge(dimStop).DistinctUntilChanged().Prepend(false);

            var brightenStart = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x is On or BrightnessMoveUp)
                .Select(_ => true);
            var brightenStop = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == BrightnessStop)
                .Select(_ => false);
            Brightening = brightenStart.Merge(brightenStop).DistinctUntilChanged().Prepend(false);
        }

        public IObservable<Unit> LeftPressed { get; }
        public IObservable<Unit> RightPressed { get; }
        public IObservable<Unit> SmallLightPressed { get; }
        public IObservable<Unit> LargeLightPressed { get; }
        public IObservable<bool> LeftHold { get; }
        public IObservable<bool> RightHold { get; }
        public IObservable<bool> Dimming { get; }
        public IObservable<bool> Brightening { get; }
    }
}
