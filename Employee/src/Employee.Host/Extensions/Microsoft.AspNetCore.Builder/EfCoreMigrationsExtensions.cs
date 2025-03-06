using Employee.Infra.EFCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Microsoft.AspNetCore.Builder;

public static class EfCoreMigrationsExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return;

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();

        var retryCount = 5;
        var retryDelay = TimeSpan.FromSeconds(5);

        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                if (!context.Database.GetPendingMigrations().Any())
                    break;

                context.Database.Migrate();
                break;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocorreu um erro ao aplicar as migrations. Tentativa {Attempt} de {RetryCount}", i + 1, retryCount);
                if (i == retryCount - 1)
                    throw;

                Thread.Sleep(retryDelay);
            }
        }
    }
}