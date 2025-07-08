using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MakerSchedule.Infrastructure.Data;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development";

        // Log the environment and connection string for debugging
        Console.WriteLine($"Environment: {environment}");
        Console.WriteLine($"Connection string: {connectionString}");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Check if connection string contains SQL Server indicators
            if (connectionString?.Contains("Server=") == true || 
                (connectionString?.Contains("Data Source=") == true && connectionString.Contains("Initial Catalog=")))
            {
                // Use SQL Server for Azure
                Console.WriteLine("Using SQL Server provider");
                options.UseSqlServer(connectionString);
            }
            else
            {
                // Use SQLite for local development
                Console.WriteLine("Using SQLite provider");
                options.UseSqlite(connectionString);
            }
        });

        return services;
    }
}
