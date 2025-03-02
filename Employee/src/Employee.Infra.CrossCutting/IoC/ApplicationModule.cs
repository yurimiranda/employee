using Autofac;
using Employee.Application.Abstractions;

namespace Employee.Infra.CrossCutting.IoC;

public sealed class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(UseCaseBase).Assembly)
            .AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}