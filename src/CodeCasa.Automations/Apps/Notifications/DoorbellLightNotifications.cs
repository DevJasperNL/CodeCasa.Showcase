using System.Drawing;
using System.Reactive.Concurrency;
using CodeCasa.Automations.Nodes;
using CodeCasa.CustomEntities.Core.Doorbell;
using CodeCasa.NetDaemon.Extensions.Observables;
using CodeCasa.Notifications.Lights;
using NetDaemon.AppModel;
using Reactive.Boolean;

namespace CodeCasa.Automations.Apps.Notifications;

/// <summary>
/// Blinks every light pipeline that opted in with AddNotifications() for thirty seconds when the doorbell is pressed.
/// This app knows nothing about rooms or pipelines; it only raises and removes a notification.
/// </summary>
[NetDaemonApp]
internal class DoorbellLightNotifications
{
    public DoorbellLightNotifications(
        FrontDoorDoorbell frontDoorDoorbell,
        LightNotificationContext lightNotifications,
        IScheduler scheduler)
    {
        const string notificationId = nameof(DoorbellLightNotifications);

        frontDoorDoorbell.BellPressed
            .PersistTrueFor(TimeSpan.FromSeconds(30), scheduler)
            .SubscribeTrueFalse(
                () => lightNotifications.Notify(notificationId, sp => new BlinkNode(scheduler, Color.Blue, Color.White), priority: 10),
                () => lightNotifications.RemoveNotification(notificationId));
    }
}
