using Employee.Application.Abstractions;
using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(UseCaseBase).Assembly,
            filter: s => !s.ValidatorType.IsGenericType
        );
        return services;
    }
}