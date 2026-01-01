using CodeCasa.Notifications.InputSelect.NetDaemon.Interact;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCasa.CustomEntities.Automation.Notifications.Dashboards;

public class LivingRoomPanelDashboardNotifications(
    [FromKeyedServices("input_select.living_room_panel_notifications")] IInputSelectNotificationEntity inputSelectNotifications)
    : InputSelectNotificationEntity(inputSelectNotifications);