using System.Reactive.Concurrency;
using CodeCasa.Automations.Nodes;
using CodeCasa.Notifications.Light;
using NetDaemon.AppModel;

namespace CodeCasa.Automations.Apps.Notifications;

[NetDaemonApp]
internal class AlarmLight
{
    public AlarmLight(
        IScheduler scheduler,
        LightNotificationContext lightNotification)
    {
        lightNotification.Notify<AlarmNode>("alarm");
    }
}