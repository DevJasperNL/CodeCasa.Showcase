using System.Drawing;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CodeCasa.AutomationPipelines.Lights.Nodes;
using CodeCasa.Lights;
using CodeCasa.Lights.NetDaemon;

namespace CodeCasa.Automations.Nodes
{
    public class AlarmNode : LightTransitionNode, IDisposable
    {
        private readonly IDisposable _subscription;

        public AlarmNode(IScheduler scheduler, NetDaemonLight light) : base(scheduler)
        {
            var high = false;
            _subscription = Observable.Interval(TimeSpan.FromSeconds(2), scheduler)
                .Subscribe(_ =>
                {
                    high = !high;
                    Output = new LightParameters{RgbColor = high ? Color.Red : Color.Yellow}.AsTransitionInSeconds(0);
                });
        }

        public void Dispose()
        {// todo: test
            _subscription.Dispose();
        }
    }
}
