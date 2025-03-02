using Employee.Infra.CrossCutting.IoC;
using Employee.Infra.EFCore.Abstractions;
using Mapster;

namespace Microsoft.Extensions.DependencyInjection;

public static class MapsterExtensions
{
    public static IServiceCollection AddMapsterService(this IServiceCollection services)
    {
        services.AddMapster();
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ApplicationModule).Assembly, typeof(DtoBase<,>).Assembly);
        return services;
    }
}