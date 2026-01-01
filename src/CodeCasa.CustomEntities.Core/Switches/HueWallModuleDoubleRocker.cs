using System.Reactive;
using System.Reactive.Linq;
using CodeCasa.CustomEntities.Core.Extensions;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Core.Switches
{
    public abstract class HueWallModuleDoubleRocker
    {
        private const string LeftPress = "left_press";
        private const string RightPress = "right_press";

        protected HueWallModuleDoubleRocker(ITriggerManager triggerManager, string mqttDeviceName, bool invertButtons = false)
        {
            LeftPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == (invertButtons ? RightPress : LeftPress))
                .Select(_ => Unit.Default);
            RightPressed = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == (invertButtons ? LeftPress : RightPress))
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> LeftPressed { get; }
        public IObservable<Unit> RightPressed { get; }
    }
}
