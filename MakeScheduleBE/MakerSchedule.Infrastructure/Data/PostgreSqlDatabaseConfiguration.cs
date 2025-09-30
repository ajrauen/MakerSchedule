using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MakerSchedule.Infrastructure.Data;

public static class PostgreSqlDatabaseConfiguration
{
    public static IServiceCollection AddPostgreSqlDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["DefaultPostgreSQLConnection"] ?? configuration.GetConnectionString("DefaultConnection");
        var environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development";

        // Log the environment and connection string for debugging
        Console.WriteLine($"Environment: {environment}");
        Console.WriteLine($"PostgreSQL Connection string: {connectionString}");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });
        });

        return services;
    }
}