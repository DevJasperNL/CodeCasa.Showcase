
using System.Reactive;
using System.Reactive.Linq;

namespace CodeCasa.CustomEntities.Automation.People;

public class PeopleEntities(Jane jane, Jasper jasper)
{
    public Jane Jane { get; } = jane;
    public Jasper Jasper { get; } = jasper;

    public IEnumerable<CompositePersonEntity> All { get; } = [jane, jasper];

    public IObservable<bool> AnyAwakeWithCurrent() =>
        All.Select(e => e.PersonStateEqualsWithCurrent(PersonStates.Awake))
            .CombineLatest(x => x.Any());

    public IObservable<bool> NoOneAsleepWithCurrent() =>
        All.Select(e => e.PersonStateEqualsWithCurrent(PersonStates.Asleep))
            .CombineLatest(x => !x.Any());

    public IObservable<Unit> OnLastPersonToAsleepOrAwayObservable()
    {
        return All.Select(p => p.PersonStateChangeWithCurrent()).CombineLatest()
            .Where(tuple =>
            {
                var prevAnyAwake = tuple.Any(change => change.Old == PersonStates.Awake);
                var currentAnyAwake = tuple.Any(change => change.New == PersonStates.Awake);
                return prevAnyAwake && !currentAnyAwake;
            }).DistinctUntilChanged().Select(_ => Unit.Default);
    }
}