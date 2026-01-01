using System.Drawing;
using System.Reactive.Concurrency;
using CodeCasa.AutomationPipelines;
using CodeCasa.Lights;

namespace CodeCasa.Automations.Apps.Lights.OfficeLights
{
    internal class ColorTransitionNode : PipelineNode<LightTransition>
    {
        public ColorTransitionNode(IScheduler scheduler, TimeSpan transitionTime, params Color[] colors)
        {
            var defaultTransition = TimeSpan.FromMilliseconds(500);
            if (transitionTime < defaultTransition)
            {
                throw new ArgumentException($"Transition time should be at least {defaultTransition}.", nameof(transitionTime));
            }
            var colors1 = colors ?? throw new ArgumentNullException(nameof(colors));
            if (colors1.Length == 0)
            {
                throw new ArgumentException("At least one color must be provided.", nameof(colors));
            }

            var index = 0;
            Output = new LightParameters
            {
                RgbColor = colors1[0],
                Brightness = byte.MaxValue
            }.AsTransition();

            scheduler.Schedule(defaultTransition, () =>
            {
                index = (index + 1) % colors1.Length;
                Output = new LightParameters
                {
                    RgbColor = colors1[index],
                    Brightness = byte.MaxValue
                }.AsTransition(transitionTime - defaultTransition);
            });

            scheduler.SchedulePeriodic(
                transitionTime,
                () =>
                {
                    index = (index + 1) % colors1.Length;
                    Output = new LightParameters
                    {
                        RgbColor = colors1[index],
                        Brightness = byte.MaxValue
                    }.AsTransition(transitionTime);
                });
        }
    }
}
