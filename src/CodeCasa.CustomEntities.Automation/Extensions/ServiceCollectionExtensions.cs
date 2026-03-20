using CodeCasa.CustomEntities.Automation.Notifications.Dashboards;
using CodeCasa.CustomEntities.Automation.Notifications.Phones;
using CodeCasa.CustomEntities.Automation.People;
using CodeCasa.CustomEntities.Automation.Sensors;
using CodeCasa.CustomEntities.Automation.Switches;
using CodeCasa.CustomEntities.Core.Extensions;
using CodeCasa.Notifications.Lights.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCasa.CustomEntities.Automation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeCasaCustomAutomationEntities(this IServiceCollection serviceCollection)
    {
        return serviceCollection

            .AddCodeCasaCustomCoreEntities()
            .AddLightNotifications()

            .AddTransient<AtticWallSwitch>()
            .AddTransient<BathroomWallSwitch>()
            .AddTransient<OfficeWallSwitch>()
            .AddTransient<AtticDimmerSwitch>()
            .AddTransient<OfficeDimmerSwitch>()

            .AddTransient<BathroomMotionSensor>()
            .AddTransient<OfficeMotionSensor>()
            .AddTransient<UpstairsHallwayAtticMotionSensor>()

            // Dashboard Notifications
            .AddTransient<LivingRoomPanelDashboardNotifications>()
            .AddTransient<JaneDashboardNotifications>()
            .AddTransient<JasperDashboardNotifications>()

            // Phone Notifications
            .AddTransient<JanePhoneNotifications>()
            .AddTransient<JasperPhoneNotifications>()
            
            // People
            .AddTransient<Jane>()
            .AddTransient<Jasper>()
            .AddTransient<PeopleEntities>();
    }
}