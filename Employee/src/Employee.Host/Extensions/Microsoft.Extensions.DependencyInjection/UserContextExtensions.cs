using Employee.Domain.Services;
using Employee.Host.Middlewares.UserContext;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class UserContextExtensions
{
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.TryAddScoped<IUserContextAccessor, UserContextAccessor>();
        services.TryAddScoped<IUserContextProvider, UserContextAccessor>();
        return services;
    }
}