using CodeCasa.CustomEntities.Automation.People;
using NetDaemon.AppModel;
using Reactive.Boolean;

namespace CodeCasa.Automations.Apps.People;

/// <summary>
/// This app synchronizes the state of people in the home with their home presence.
/// </summary>
[NetDaemonApp]
internal class PeopleHomeStateSyncer
{
    public PeopleHomeStateSyncer(PeopleEntities people)
    {
        foreach (var person in people.All)
        {
            person.HomeWithCurrent()
                .SubscribeTrueFalse(
                    () =>
                    {
                        if (person.State != PersonStates.Away)
                        {
                            return;
                        }

                        person.State = PersonStates.Awake;
                    },
                    () =>
                    {
                        if (person.State == PersonStates.Away)
                        {
                            return;
                        }

                        person.State = PersonStates.Away;
                    });
        }
    }
}