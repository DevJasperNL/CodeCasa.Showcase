using CodeCasa.AutomationPipelines;
using CodeCasa.AutomationPipelines.Lights.Nodes;
using CodeCasa.AutomationPipelines.Lights.ReactiveNode;
using CodeCasa.CustomEntities.Automation.People;
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
    }
}
