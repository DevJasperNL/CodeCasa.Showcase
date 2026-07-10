using System.Reactive.Concurrency;
using Occurify;
using Occurify.Astro;
using Occurify.Extensions;
using Occurify.Reactive.Extensions;
using Occurify.TimeZones;
using Reactive.Boolean;

namespace CodeCasa.Automations.Apps.Lights.BackyardLights.Observables;

public class BackyardLightsRoutine : IObservable<bool>
{
    private readonly IObservable<bool> _observable;

    public BackyardLightsRoutine(IScheduler scheduler, TimeSpan offset)
    {
        var eveningAndNight = PeriodTimeline.Between(AstroInstants.LocalSunsets.Offset(offset), TimeZoneInstants.DailyAt(2)).ToBooleanObservable(scheduler);
        var morning = TimeZonePeriods.DailyBetween(TimeZoneInstants.DailyAt(6).Offset(offset), AstroInstants.LocalSunrises).ToBooleanObservable(scheduler);

        _observable = eveningAndNight.Or(morning);
    }

    public IDisposable Subscribe(IObserver<bool> observer)
    {
        return _observable.Subscribe(observer);
    }
}