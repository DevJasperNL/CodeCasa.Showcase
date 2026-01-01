using CodeCasa.Notifications.InputSelect.NetDaemon.Interact;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCasa.CustomEntities.Automation.Notifications.Dashboards;

public class JaneDashboardNotifications(
    [FromKeyedServices("input_select.jane_notifications")] IInputSelectNotificationEntity inputSelectNotifications)
    : InputSelectNotificationEntity(inputSelectNotifications);