using CodeCasa.CustomEntities.Automation.Notifications.Dashboards;
using CodeCasa.CustomEntities.Automation.Notifications.Phones;
using CodeCasa.CustomEntities.Automation.People;
using CodeCasa.CustomEntities.Automation.Sensors;
using CodeCasa.CustomEntities.Automation.Switches;
using CodeCasa.CustomEntities.Core.Extensions;
using CodeCasa.CustomEntities.Core.Switches;
using CodeCasa.NetDaemon.Sensors.Composite;
using CodeCasa.Notifications.Lights.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCasa.CustomEntities.Automation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeCasaCustomAutomationEntities(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddCodeCasaCustomCoreEntities()
            .AddLightNotifications();

        serviceCollection
            .AddTransientImplementationsOf<HueDimmerSwitch>()
            .AddTransientImplementationsOf<HueWallModuleDoubleRocker>()
            .AddTransientImplementationsOf<HueWallModuleSingleRocker>()
            .AddTransientImplementationsOf<IkeaStyrbarRemoteControl>()
            .AddTransientImplementationsOf<IkeaRodretDimmer>();

        serviceCollection.AddTransientImplementationsOf<MotionSensor>();

        // Dashboard Notifications
        serviceCollection.AddTransient<LivingRoomPanelDashboardNotifications>()
            .AddTransient<JaneDashboardNotifications>()
            .AddTransient<JasperDashboardNotifications>()

            // Phone Notifications
            .AddTransient<JanePhoneNotifications>()
            .AddTransient<JasperPhoneNotifications>()
            
            // People
            .AddTransientImplementationsOf<CompositePersonEntity>()
            .AddTransient<PeopleEntities>();

        return serviceCollection;
    }

    private static IServiceCollection AddTransientImplementationsOf<TBase>(this IServiceCollection services)
    {
        var baseType = typeof(TBase);

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => baseType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).ToArray();

        foreach (var type in types)
        {
            services.AddTransient(type);
        }

        return services;
    }
}