using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System.Text.Json;

namespace Microsoft.AspNetCore.Builder;

public static class SerilogExtensions
{
    public static IApplicationBuilder UseLogs(this IApplicationBuilder builder)
    {
        return builder
            .UseResponsePreparation()
            .UseSerilogRequestLogging(logConfig =>
            {
                logConfig.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms" + Environment.NewLine + "Request: {RequestBody}" + Environment.NewLine + "Response: {ResponseBody}";
                logConfig.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("RequestProtocol", httpContext.Request.Protocol);
                };
                logConfig.IncludeQueryInRequestPath = true;
                logConfig.GetMessageTemplateProperties = (httpContext, requestPath, elapsedMs, statusCode) =>
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
                        var structureValue = GetBody(httpContext.Request.Body);
                        events.Add(new LogEventProperty("RequestBody", structureValue));
                    }
                    else
                    {
                        events.Add(new LogEventProperty("RequestBody", new ScalarValue("<no body>")));
                    }

                    if (httpContext.Response.Body.CanRead)
                    {
                        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                        var structureValue = GetBody(httpContext.Response.Body);
                        events.Add(new LogEventProperty("ResponseBody", structureValue));
                    }
                    else
                    {
                        events.Add(new LogEventProperty("ResponseBody", new ScalarValue("<no body>")));
                    }

                    return events;
                };
            });
    }

    private static StructureValue GetBody(Stream stream)
    {
        var reader = new StreamReader(stream);
        var body = reader.ReadToEndAsync().GetAwaiter().GetResult();
        if (string.IsNullOrWhiteSpace(body))
            return new StructureValue([]);

        stream.Seek(0, SeekOrigin.Begin);

        var jsonDocument = JsonDocument.Parse(body);
        return new StructureValue(
            jsonDocument.RootElement.EnumerateObject().Select(
                p => new LogEventProperty(p.Name, ConvertJsonElementToLogEventPropertyValue(p.Value))));
    }

    private static LogEventPropertyValue ConvertJsonElementToLogEventPropertyValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => new StructureValue(element.EnumerateObject().Select(p =>
                new LogEventProperty(p.Name, ConvertJsonElementToLogEventPropertyValue(p.Value)))),
            JsonValueKind.Array => new SequenceValue(element.EnumerateArray().Select(ConvertJsonElementToLogEventPropertyValue)),
            JsonValueKind.String => new ScalarValue(element.GetString()),
            JsonValueKind.Number => new ScalarValue(element.GetDecimal()),
            JsonValueKind.True => new ScalarValue(true),
            JsonValueKind.False => new ScalarValue(false),
            JsonValueKind.Null => new ScalarValue(null),
            _ => new ScalarValue(element.ToString())
        };
    }
}