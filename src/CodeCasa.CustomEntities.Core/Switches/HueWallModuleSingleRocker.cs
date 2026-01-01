using System.Reactive;
using System.Reactive.Linq;
using CodeCasa.CustomEntities.Core.Extensions;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Core.Switches
{
    public abstract class HueWallModuleSingleRocker : IObservable<Unit>
    {
        private const string LeftPress = "left_press";
        private const string RightPress = "right_press";

        private readonly IObservable<Unit> _observable;

        protected HueWallModuleSingleRocker(ITriggerManager triggerManager, string mqttDeviceName, bool rightSwitch = false)
        {
            _observable = triggerManager.Zigbee2MqttActions(mqttDeviceName)
                .Where(x => x == (rightSwitch ? RightPress : LeftPress))
                .Select(_ => Unit.Default);
        }

        public IDisposable Subscribe(IObserver<Unit> observer) => _observable.Subscribe(observer);
    }
}
