using Employee.Application.Abstractions;
using Employee.Infra.EFCore.Abstractions;
using Mapster;

namespace Microsoft.Extensions.DependencyInjection;

public static class MapsterExtensions
{
    public static IServiceCollection AddMapsterService(this IServiceCollection services)
    {
        services.AddMapster();
        TypeAdapterConfig.GlobalSettings.Scan(typeof(UseCaseBase).Assembly, typeof(DtoBase<,>).Assembly);
        return services;
    }
}