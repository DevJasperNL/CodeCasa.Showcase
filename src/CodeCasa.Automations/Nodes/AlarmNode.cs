using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CodeCasa.AutomationPipelines.Lights.Context;
using CodeCasa.AutomationPipelines.Lights.Nodes;

namespace CodeCasa.Automations.Nodes
{
    public class AlarmNode : LightTransitionNode, IDisposable
    {
        private readonly IDisposable _subscription;

        public AlarmNode(IScheduler scheduler, ILightPipelineContext lightPipelineContext) : base(scheduler)
        {
            var high = false;
            _subscription = Observable.Interval(TimeSpan.FromSeconds(2), scheduler)
                .Subscribe(_ =>
                {
                    high = !high;
                    //Output = lightPipelineContext.LightEntity.GetWarningSceneParameters(high).AsTransitionInSeconds(0);
                });
        }

        public void Dispose()
        {// todo: test
            _subscription.Dispose();
        }
    }
}
