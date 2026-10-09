using System.Drawing;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CodeCasa.AutomationPipelines.Lights.Nodes;
using CodeCasa.Lights;

namespace CodeCasa.Automations.Nodes;

/// <summary>
/// A pipeline node that alternates between two colours at full brightness until it is disposed.
/// </summary>
public class BlinkNode : LightTransitionNode
{
    private readonly IDisposable _timer;

    public BlinkNode(IScheduler scheduler, Color firstColor, Color secondColor, TimeSpan? interval = null) : base(scheduler)
    {
        Name = "Blink";
        LightTransition[] states =
        [
            new LightParameters { Brightness = 255, RgbColor = firstColor }.AsTransition(),
            new LightParameters { Brightness = 255, RgbColor = secondColor }.AsTransition()
        ];

        // Output is serialised by the base class, so setting it from the timer is safe.
        Output = states[0];
        _timer = Observable.Interval(interval ?? TimeSpan.FromSeconds(1), scheduler)
            .Subscribe(tick => Output = states[(int)((tick + 1) % 2)]);
    }

    public override ValueTask DisposeAsync()
    {
        _timer.Dispose();
        return base.DisposeAsync();
    }
}
