using Carter;

namespace Microsoft.AspNetCore.Builder;

public static class AppBuilderExtensions
{
    public static void ConfigureApp(this WebApplication app)
    {
        app.ApplyMigrations();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerService();
        }

        app.UseLogs();
        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseUserContext();
        app.MapCarter();
    }
}