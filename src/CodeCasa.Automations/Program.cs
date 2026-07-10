using System.Reflection;
using CodeCasa.Automations.Extensions;
using Microsoft.Extensions.Hosting;
using NetDaemon.AppModel;
using NetDaemon.Extensions.Scheduler;
using NetDaemon.Runtime;
using Serilog;

#pragma warning disable CA1812

try
{
    // This ensures that the log files are written relative the application's launch folder.
    Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

    await Host.CreateDefaultBuilder(args)
        .UseCodeCasa()
        .UseNetDaemonRuntime()
        .ConfigureServices((context, services) =>
            services
                .AddAppsFromAssembly(Assembly.GetExecutingAssembly())
                .AddNetDaemonScheduler()
                .AddCodeCasa(context.Configuration)
        )
        .Build()
        .RunAsync()
        .ConfigureAwait(false);
}
catch (Exception e)
{
    Log.Logger.Error($"Failed to start host... {e}");
    throw;
}