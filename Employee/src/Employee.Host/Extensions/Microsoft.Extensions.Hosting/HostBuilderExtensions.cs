using Autofac;
using Autofac.Extensions.DependencyInjection;
using Employee.Infra.CrossCutting.IoC;
using Serilog;

namespace Microsoft.Extensions.Hosting;

public static class HostBuilderExtensions
{
    public static void ConfigureHost(this IHostBuilder host)
    {
        host.UseSerilog();
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(
            builder => builder.RegisterModule(new ApplicationModule()));
    }
}