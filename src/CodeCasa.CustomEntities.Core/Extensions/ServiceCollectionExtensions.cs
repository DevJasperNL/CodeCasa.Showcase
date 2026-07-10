using CodeCasa.CustomEntities.Core.Doorbell;
using CodeCasa.CustomEntities.Core.GoogleHome;
using CodeCasa.CustomEntities.Core.InputSelect;
using CodeCasa.CustomEntities.Core.Weather;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCasa.CustomEntities.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeCasaCustomCoreEntities(this IServiceCollection serviceCollection)
    {
        return serviceCollection

            .AddTransientImplementationsOf<ReolinkDoorbell>()

            .AddTransient<GoogleHomeAlarmEntities>()
            .AddTransient<GoogleHomeTimerEntities>()

            // Input Select Entities
            .AddTransient<LivingRoomWallPanelView>()
            
            .AddTransient<ForecastHome>();
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