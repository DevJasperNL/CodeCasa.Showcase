using CodeCasa.AutomationPipelines.Lights.Nodes;
using CodeCasa.Lights;
using Occurify;
using Occurify.TimeZones;
using System.Reactive.Concurrency;
using CodeCasa.Lights.Timelines.Extensions;

namespace CodeCasa.Automations.Apps.Lights.BathroomLights
{
    internal class BathroomLightTimelineNode : LightTransitionNode
    {
        public BathroomLightTimelineNode(IScheduler scheduler) : base(scheduler)
        {
            var timeline = new Dictionary<ITimeline, LightParameters>
            {
                { TimeZoneInstants.DailyAt(4), LightParameters.Dimmed },
                { TimeZoneInstants.DailyAt(5), LightParameters.Bright },
                { TimeZoneInstants.DailyAt(16), LightParameters.Bright },
                { TimeZoneInstants.DailyAt(23), LightParameters.Dimmed }
            };

            timeline.ToLightTransitionObservableIncludingCurrent(scheduler)
                .Subscribe(transition =>
                {
                    Output = transition;
                });
        }
    }
}
