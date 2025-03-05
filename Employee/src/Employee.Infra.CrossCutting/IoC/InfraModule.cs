using Autofac;
using Employee.Infra.EFCore;
using Employee.Infra.Jwt.Configurations;

namespace Employee.Infra.CrossCutting.IoC;

public sealed class InfraModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(ApplicationDbContext).Assembly)
            .AsImplementedInterfaces().InstancePerLifetimeScope();

        builder
            .RegisterAssemblyTypes(typeof(JwtConfiguration).Assembly)
            .AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}