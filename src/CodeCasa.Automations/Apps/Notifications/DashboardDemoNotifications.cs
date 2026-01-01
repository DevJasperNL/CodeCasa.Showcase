using CodeCasa.CustomEntities.Automation.Notifications.Dashboards;
using CodeCasa.CustomEntities.Core.Events;
using CodeCasa.NetDaemon.Utilities.Extensions;
using NetDaemon.AppModel;
using NetDaemon.HassModel;
using NetDaemon.Notifications.InputSelect;

namespace CodeCasa.Automations.Apps.Notifications;

[NetDaemonApp]
internal class DashboardDemoNotifications
{
    private List<InputSelectNotification> _notifications = [];
    private int _manuallyAddedIndex;

    public DashboardDemoNotifications(
        IHaContext haContext,
        LivingRoomPanelDashboardNotifications livingRoomPanelNotifications)
    {
        haContext.Events.Filter(Events.LivingRoomPanelNotificationDemoEvent).Subscribe(_ =>
        {
            _notifications.Clear();
            _manuallyAddedIndex = 0;

            var clickToRemoveNotificationId = $"{nameof(DashboardDemoNotifications)}_ClickToRemove";
            var clickToUndoRemoveNotificationId = $"{nameof(DashboardDemoNotifications)}_ClickToUndoRemove";

            void AddClickToRemoveNotificationAction() =>
                _notifications.Add(livingRoomPanelNotifications.Notify(new LivingRoomPanelNotificationConfig
                {
                    Order = 900,
                    Message = "Demo Notification 1",
                    SecondaryMessage = "Click to remove me.",
                    Icon = "Material.Filled.AutoAwesome",
                    Action = () =>
                    {
                        livingRoomPanelNotifications.RemoveNotification(clickToRemoveNotificationId);
                        _notifications = _notifications.Where(n => n.Id != clickToRemoveNotificationId).ToList();

                        _notifications.Add(livingRoomPanelNotifications.Notify(new LivingRoomPanelNotificationConfig
                        {
                            Message = "Notification removed!",
                            SecondaryMessage = "Click to undo remove.",
                            Icon = "Material.Filled.Undo",
                            Timeout = TimeSpan.FromSeconds(3),
                            Action = () =>
                            {
                                livingRoomPanelNotifications.RemoveNotification(clickToUndoRemoveNotificationId);
                                _notifications = _notifications.Where(n => n.Id != clickToUndoRemoveNotificationId)
                                    .ToList();

                                AddClickToRemoveNotificationAction();
                            }
                        }, clickToUndoRemoveNotificationId));
                    }
                }, clickToRemoveNotificationId));

            AddClickToRemoveNotificationAction();

            var addNotificationNotificationId = $"{nameof(DashboardDemoNotifications)}_Add";

            void AddNotificationNotificationAction() => _notifications.Add(livingRoomPanelNotifications.Notify(
                new LivingRoomPanelNotificationConfig
                {
                    Order = 901,
                    Message = "Demo Notification 3",
                    SecondaryMessage = "Click to add notification.",
                    Icon = "Material.Filled.AutoAwesome",
                    Action = () =>
                    {
                        _manuallyAddedIndex++;
                        var notificationId = Guid.NewGuid().ToString();
                        _notifications.Add(livingRoomPanelNotifications.Notify(new LivingRoomPanelNotificationConfig
                        {
                            Order = 902,
                            Message = $"Added Demo Notification ({_manuallyAddedIndex})",
                            SecondaryMessage = "Click to remove me.",
                            Icon = "Material.Filled.AutoAwesome",
                            Action = () =>
                            {
                                livingRoomPanelNotifications.RemoveNotification(notificationId);
                                _notifications = _notifications.Where(n => n.Id != notificationId).ToList();
                            }
                        }, notificationId));
                        AddNotificationNotificationAction();
                    }
                }, addNotificationNotificationId));

            AddNotificationNotificationAction();

            var clearNotificationId = $"{nameof(DashboardDemoNotifications)}_Clear";
            _notifications.Add(livingRoomPanelNotifications.Notify(new LivingRoomPanelNotificationConfig
            {
                Order = 903,
                Message = "Demo Notification 2",
                SecondaryMessage = "Click to clear demo notifications.",
                Icon = "Material.Filled.AutoAwesome",
                Action = () =>
                {
                    foreach (var notification in _notifications)
                    {
                        livingRoomPanelNotifications.RemoveNotification(notification);
                    }

                    _notifications.Clear();
                }
            }, clearNotificationId));
        });
    }
}