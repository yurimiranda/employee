namespace Microsoft.AspNetCore.Builder;

public static class ResponseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ResponseLoggingPreparationMiddleware>();
    }
}

public class ResponseLoggingPreparationMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        using var originalBody = httpContext.Response.Body;
        using var responseBody = new MemoryStream();
        httpContext.Response.Body = responseBody;

        await next(httpContext);

        await responseBody.CopyToAsync(originalBody);
        httpContext.Response.Body = originalBody;
    }
}