using Carter;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection;

public static class CrossCuttingExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.ConfigureHttpJsonOptions(
            options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        services.Configure<AspNetCore.Mvc.JsonOptions>(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddCarter();
        services.AddSwaggerService();
        services.AddCors(c => c.AddDefaultPolicy(d => d.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().SetIsOriginAllowed(o => true)));
        services.AddLogs(configuration);
        services.AddValidation();
        services.AddMapsterService();
        services.AddAuthentications(configuration);
        return services;
    }
}