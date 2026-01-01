using System.Reactive.Concurrency;
using CodeCasa.AutomationPipelines;
using CodeCasa.AutomationPipelines.Lights.Context;
using CodeCasa.Lights;

namespace CodeCasa.Automations.Apps.Lights.OfficeLights
{
    internal class TestNode : PipelineNode<LightTransition>
    {
        public TestNode(ILightPipelineContext context, IScheduler scheduler)
        {
            scheduler.SchedulePeriodic(
                TimeSpan.FromSeconds(2),
                () => Console.WriteLine($"Tick at {DateTime.Now} for {context.LightEntity.Id}")
            );
        }
    }
}
