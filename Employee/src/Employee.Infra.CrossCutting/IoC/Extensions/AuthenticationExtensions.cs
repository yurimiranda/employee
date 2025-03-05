using Employee.Infra.EFCore;
using Employee.Infra.Jwt.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationExtensions
{
    public static void AddAuthentications(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection(JwtConfiguration.Section));

        var jwtConfig = configuration.GetSection(JwtConfiguration.Section).Get<JwtConfiguration>() ?? new();

        services.AddJwksManager().PersistKeysToDatabaseStore<ApplicationDbContext>();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = jwtConfig.ValidateIssuer,
                ValidateLifetime = jwtConfig.ValidateLifetime,
                ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                ValidateAudience = jwtConfig.ValidateAudience,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                LifetimeValidator = (before, expires, token, param) => expires!.Value > DateTime.UtcNow,
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var lastKeys = serviceProvider.GetRequiredService<IJwtService>().GetLastKeys();
                    return lastKeys.GetAwaiter().GetResult().Select(r => r.GetSecurityKey());
                },
                RoleClaimType = "role"
            };
        });
        services.AddAuthorization();
    }
}