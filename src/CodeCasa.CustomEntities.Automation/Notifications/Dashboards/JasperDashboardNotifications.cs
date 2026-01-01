using CodeCasa.Notifications.InputSelect.NetDaemon.Interact;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCasa.CustomEntities.Automation.Notifications.Dashboards;

public class JasperDashboardNotifications(
    [FromKeyedServices("input_select.jasper_notifications")] IInputSelectNotificationEntity inputSelectNotifications)
    : InputSelectNotificationEntity(inputSelectNotifications);