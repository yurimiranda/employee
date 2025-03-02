using Serilog;
using Serilog.Enrichers.Sensitive;

namespace Microsoft.Extensions.DependencyInjection;

public static class SerilogExtensions
{
    public static IServiceCollection AddLogs(this IServiceCollection services, IConfigurationRoot configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithSensitiveDataMasking(options => options.MaskProperties.Add("Password"))
            .CreateLogger();
        return services;
    }

    public static IApplicationBuilder UseLogs(this IApplicationBuilder builder)
    {
        return builder.UseSerilogRequestLogging();
    }
}