using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class SerilogExtensions
{
    public static IServiceCollection AddLogs(this IServiceCollection services, IConfigurationRoot configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithSensitiveDataMasking(options => options.MaskProperties.Add("Password"))
            .CreateLogger();
        services.AddLogging(config => config.ClearProviders().AddProvider(new SerilogLoggerProvider(Log.Logger)));
        return services;
    }

    public static IApplicationBuilder UseLogs(this IApplicationBuilder builder)
    {
        return builder.UseSerilogRequestLogging(static logConfig =>
        {
            logConfig.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms" + Environment.NewLine + "Body: {RequestBody}" + Environment.NewLine + "Response: {ResponseBody}";
            logConfig.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("RequestProtocol", httpContext.Request.Protocol);
            };
            logConfig.IncludeQueryInRequestPath = true;
            logConfig.GetMessageTemplateProperties = static (httpContext, requestPath, elapsedMs, statusCode) =>
            {
                var events = new List<LogEventProperty>
                {
                    new("RequestMethod", new ScalarValue(httpContext.Request.Method)),
                    new("RequestPath", new ScalarValue(requestPath)),
                    new("StatusCode", new ScalarValue(statusCode)),
                    new("Elapsed", new ScalarValue(elapsedMs))
                };

                if (httpContext.Request.Body.CanRead)
                {
                    httpContext.Request.EnableBuffering();
                    var reader = new StreamReader(httpContext.Request.Body);
                    var body = reader.ReadToEndAsync();
                    body.Wait();
                    httpContext.Request.Body.Position = 0;
                    events.Add(new LogEventProperty("RequestBody", new ScalarValue(body.Result)));
                }
                else
                {
                    events.Add(new LogEventProperty("RequestBody", new ScalarValue("<no body>")));
                }

                if (httpContext.Response.Body.CanRead)
                {
                    httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(httpContext.Response.Body);
                    var body = reader.ReadToEndAsync();
                    body.Wait();
                    httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                    events.Add(new LogEventProperty("ResponseBody", new ScalarValue(body.Result)));
                }
                else
                {
                    events.Add(new LogEventProperty("ResponseBody", new ScalarValue("<no body>")));
                }

                return events;
            };
        });
    }
}