using Carter;

namespace Microsoft.AspNetCore.Builder;

public static class AppBuilderExtensions
{
    public static void ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerService();
        }

        app.UseCustomExceptionHandler();
        app.UseLogs();
        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapCarter();
    }
}