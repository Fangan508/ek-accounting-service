using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;

namespace AccountingService.Host.Extensions;

/// <summary>
/// Provides extension methods for applying database migrations at application startup.
/// </summary>
public static class DataMigrationExtension
{
    /// <summary>
    /// Checks if migrations are necessary and applies them if needed.
    /// </summary>
    /// <param name="app">The application instance</param>
    public static void ApplyNecessaryDatabaseMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AccountingDbContext>();

            try
            {
                logger.LogInformation("Checking and applying database migrations...");
                dbContext.Database.Migrate();
                logger.LogInformation("Database migrations applied successfully.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogCritical(ex, "PostgreSQL connection or execution error. Check DB availability and credentials.");
            }
            catch (DbUpdateException ex)
            {
                logger.LogCritical(ex, "Failed to update the database schema. Possible migration or data conflict.");
            }
            catch (DbException ex)
            {
                logger.LogCritical(ex, "General database error occurred.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during database migration.");
            }
        }
    }
}