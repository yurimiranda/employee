using Autofac.Core.Registration;
using Employee.Infra.CrossCutting.IoC;

namespace Autofac;

public static class AutofacExtensions
{
    public static IModuleRegistrar RegisterModules(this ContainerBuilder builder)
    {
        return builder
            .RegisterModule(new ApplicationModule())
            .RegisterModule(new InfraModule());
    }
}