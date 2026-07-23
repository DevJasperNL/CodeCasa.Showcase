using System.Reactive.Concurrency;

namespace CodeCasa.Automations.Apps.Lights.BackyardLights.Observables
{
    internal class BackyardLightsRoutineFactory(IScheduler scheduler)
    {
        public BackyardLightsRoutine Create()
        {
            return new BackyardLightsRoutine(scheduler, TimeSpan.Zero);
        }

        public BackyardLightsRoutine Create(TimeSpan offset)
        {
            return new BackyardLightsRoutine(scheduler, offset);
        }

        public BackyardLightsRoutine Create(int offsetInSeconds)
        {
            return new BackyardLightsRoutine(scheduler, TimeSpan.FromSeconds(offsetInSeconds));
        }
    }
}
