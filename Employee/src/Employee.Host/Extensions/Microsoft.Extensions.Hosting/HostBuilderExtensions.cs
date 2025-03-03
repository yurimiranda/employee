using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace Microsoft.Extensions.Hosting;

public static class HostBuilderExtensions
{
    public static void ConfigureHost(this IHostBuilder host)
    {
        host.UseSerilog();
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(
            builder => builder.RegisterModules());
    }
}