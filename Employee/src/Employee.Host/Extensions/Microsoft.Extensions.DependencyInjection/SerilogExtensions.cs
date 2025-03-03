using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class SerilogExtensions
{
    public static IServiceCollection AddLogs(this IServiceCollection services, IConfigurationRoot configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithSensitiveDataMasking(options => options.MaskProperties.Add("Password"))
            .CreateLogger();
        services.AddLogging(config => config.ClearProviders().AddProvider(new SerilogLoggerProvider(Log.Logger)));
        return services;
    }

    public static IApplicationBuilder UseLogs(this IApplicationBuilder builder)
    {
        return builder.UseSerilogRequestLogging();
    }
}