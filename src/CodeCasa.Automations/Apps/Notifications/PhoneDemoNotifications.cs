using NetDaemon.AppModel;
using NetDaemon.HassModel;
using NetDaemon.Notifications.Phone.Config;
using System.Drawing;
using CodeCasa.CustomEntities.Automation.Notifications.Phones;
using CodeCasa.CustomEntities.Core.Events;
using CodeCasa.NetDaemon.Utilities.Extensions;

namespace CodeCasa.Automations.Apps.Notifications;

[NetDaemonApp]
internal class PhoneDemoNotifications
{
    private readonly JasperPhoneNotifications _jasperPhone;
    private const string FirstMessage = "This is a test notification";
    private const string SecondMessage = "Now my text is different";
    private int _notificationCount;

    public PhoneDemoNotifications(
        IHaContext haContext,
        JasperPhoneNotifications jasperPhone)
    {
        _jasperPhone = jasperPhone;
        
        haContext.Events.Filter(Events.PhoneNotificationDemoEvent).Subscribe(_ =>
        {
            AddOrUpdatePhoneNotification(null, Color.Yellow, FirstMessage);
        });
    }

    private void AddOrUpdatePhoneNotification(int? optionalId, Color? color, string message)
    {
        var id = optionalId ?? ++_notificationCount;
        var notificationId = $"{nameof(PhoneDemoNotifications)}_Notification_{id}";

        _jasperPhone.Notify(new AndroidNotificationConfig
        {
            Title = $"Demo Notification {id}",
            StatusBarIcon = "mdi:creation",
            Message = message,
            Color = color,
            Actions =
            [
                new(() => AddOrUpdatePhoneNotification(id, color == Color.Yellow ? Color.Blue : Color.Yellow, message), "Change color!"),
                new(() => AddOrUpdatePhoneNotification(id, color, message == FirstMessage ? SecondMessage : FirstMessage), "Change message!"),
                new(() => AddOrUpdatePhoneNotification(null, Color.Yellow, FirstMessage), "Add notification!")
            ],
            Sticky = true // Prevents notification from closing after clicking a button.
        }, notificationId);
    }
}