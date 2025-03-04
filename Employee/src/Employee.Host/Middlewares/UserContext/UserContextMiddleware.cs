using Employee.Host.Middlewares.UserContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.AspNetCore.Builder;

public static class UserContextMiddlewareExtensions
{
    public static IApplicationBuilder UseUserContext(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UserContextMiddleware>();
    }
}

public class UserContextMiddleware(
    RequestDelegate next,
    IUserContextProvider userIdProvider,
    ILogger<UserContextMiddleware> logger)
{
    public async Task Invoke(HttpContext httpContext, IMemoryCache cache)
    {
        var endpoint = httpContext.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
        {
            await next(httpContext);
            return;
        }

        if (endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() is null)
        {
            await next(httpContext);
            return;
        }

        var authId = httpContext.User.FindFirst("user_id")?.Value;
        var cacheKey = "User" + authId;
        var userContext = await cache.GetOrCreateAsync(
            cacheKey,
            async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                return await userIdProvider.GenerateUserContext(authId);
            });

        if (userContext is null)
        {
            cache.Remove(cacheKey);
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        logger.LogDebug("Generated UserContext: {@UserContext}", userContext);

        await next(httpContext);
    }
}