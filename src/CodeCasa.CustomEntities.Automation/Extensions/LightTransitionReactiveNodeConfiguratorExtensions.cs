using CodeCasa.AutomationPipelines;
using CodeCasa.AutomationPipelines.Lights.Nodes;
using CodeCasa.AutomationPipelines.Lights.ReactiveNode;
using CodeCasa.AutomationPipelines.Lights.Timeline;
using CodeCasa.CustomEntities.Automation.People;
using CodeCasa.CustomEntities.Core.Switches;
using CodeCasa.Lights;
using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Linq;

namespace CodeCasa.CustomEntities.Automation.Extensions
{
    public static class LightTransitionReactiveNodeConfiguratorExtensions
    {
        public static ILightTransitionReactiveNodeConfigurator<TLight> TurnOffWhenLastPersonToAsleepOrAway<TLight>(this ILightTransitionReactiveNodeConfigurator<TLight> configurator) where TLight : ILight
        {
            configurator.AddNodeSource(sp =>
            {
                var people = sp.GetRequiredService<PeopleEntities>();
                return people.OnLastPersonToAsleepOrAwayObservable()
                    .Select(_ => 
                        new Func<IServiceProvider, IPipelineNode<LightTransition>?>(
                            _ => new TurnOffThenPassThroughNode()));
            });
            return configurator;
        }

        public static ILightTransitionReactiveNodeConfigurator<TLight> AddHueDimmerSwitch<TLight>(this ILightTransitionReactiveNodeConfigurator<TLight> configurator,
            HueDimmerSwitch dimmerSwitch,
            LightParameters onPressParameters,
            params LightParameters[] scenes) where TLight : ILight
        {
            return configurator
                .AddToggle(dimmerSwitch.OnOffPressed, onPressParameters)
                .AddCycle(dimmerSwitch.ScenePressed, scenes)
                .AddReactiveDimmer(dimmerSwitch);
        }

        public static ILightTransitionReactiveNodeConfigurator<TLight> AddHueDimmerSwitch<TLight>(this ILightTransitionReactiveNodeConfigurator<TLight> configurator,
            HueDimmerSwitch dimmerSwitch,
            Action<ITimelineConfigurator> timelineConfigure,
            params LightParameters[] scenes) where TLight : ILight
        {
            return configurator
                .AddToggle(dimmerSwitch.OnOffPressed, timelineConfigure)
                .AddCycle(dimmerSwitch.ScenePressed, scenes)
                .AddReactiveDimmer(dimmerSwitch);
        }
    }
}
