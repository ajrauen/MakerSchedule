using MakerSchedule.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
public static class DatabaseInitializationExtensions
{
    public static async Task EnsureDatabaseReadyAsync(this WebApplication app, int maxAttempts = 5, int delayBetweenAttempts = 5000)
    {
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;
            
            try
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                app.Logger.LogInformation($"Attempting to connect to the database... (Attempt {attempts}/{maxAttempts})");
                
                if (await dbContext.Database.CanConnectAsync())
                {
                    app.Logger.LogInformation("Connected: Starting migrations and seeding database...");
                    await dbContext.Database.MigrateAsync();
                    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
                    await seeder.SeedAsync();
                    app.Logger.LogInformation("Connected: Finished migrations and seeding database.");
                    return;
                }
            }
            catch (Exception ex)
            {
                app.Logger.LogWarning($"Database connection attempt {attempts} failed: {ex.Message}");
            }
            
            if (attempts >= maxAttempts)
            {
                app.Logger.LogError("Failed to connect to the database after multiple attempts.");
                throw new InvalidOperationException("Database connection failed. Application cannot start without database access.");
            }
            
            app.Logger.LogInformation($"Waiting {delayBetweenAttempts}ms before retry...");
            await Task.Delay(delayBetweenAttempts);
        }
    }
}