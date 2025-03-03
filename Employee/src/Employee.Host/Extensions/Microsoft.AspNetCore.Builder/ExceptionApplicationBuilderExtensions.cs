﻿using Employee.Host;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using System.Text.Json;

namespace Microsoft.AspNetCore.Builder;

public static class ExceptionApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseExceptionHandler(app =>
        {
            app.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var httpRequestIdentifierFeature = context.Features.Get<IHttpRequestIdentifierFeature>();
                Log.Logger.Error(exceptionHandlerFeature?.Error, httpRequestIdentifierFeature?.TraceIdentifier ?? Guid.NewGuid().ToString(), Messages.InternalServerError);

                context.Response.StatusCode = 500;
                string text = JsonSerializer.Serialize(new
                {
                    TraceId = httpRequestIdentifierFeature?.TraceIdentifier,
                    Error = Messages.InternalServerError,
                    InnerError = exceptionHandlerFeature?.Error.Message
                });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(text);
            });
        });
    }
}