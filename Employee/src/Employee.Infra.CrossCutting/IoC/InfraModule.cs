using Autofac;
using Employee.Infra.EFCore;

namespace Employee.Infra.CrossCutting.IoC;

public sealed class InfraModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(ApplicationDbContext).Assembly)
            .AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}