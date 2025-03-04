using Autofac;
using Employee.Application.Abstractions;
using FluentValidation;

namespace Employee.Infra.CrossCutting.IoC;

public sealed class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //// Register specific closed generic types
        //builder.RegisterGeneric(typeof(PhoneValidator<>))
        //       .As(typeof(IValidator<>))
        //       .InstancePerLifetimeScope();

        // Other registrations
        builder
            .RegisterAssemblyTypes(typeof(UseCaseBase).Assembly)
            .Where(t => typeof(UseCaseBase).IsAssignableFrom(t) && !t.IsGenericType)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}