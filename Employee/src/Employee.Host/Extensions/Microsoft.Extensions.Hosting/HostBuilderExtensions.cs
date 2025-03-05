using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using Serilog.Enrichers.Sensitive;

namespace Microsoft.Extensions.Hosting;

public static class HostBuilderExtensions
{
    public static void ConfigureHost(this IHostBuilder host)
    {
        host.UseSerilog((context, loggerConfig) =>
            loggerConfig
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.WithSensitiveDataMasking(options => options.MaskProperties.Add("Password")));

        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(
            builder => builder.RegisterModules());
    }
}