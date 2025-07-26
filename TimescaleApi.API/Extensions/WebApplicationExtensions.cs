using TimescaleApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TimescaleApi.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider
                .GetRequiredService<ILogger<WebApplication>>();
            var context = scope.ServiceProvider
                .GetRequiredService<TimescaleDbContext>();

            var pending = context.Database.GetPendingMigrations();
            if (pending.Any())
            {
                context.Database.Migrate();
                logger.LogInformation("Database migrations applied");
            }
        }
    }
}
