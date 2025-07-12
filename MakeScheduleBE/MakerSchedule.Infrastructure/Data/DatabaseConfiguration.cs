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
            // Always use SQL Server
            Console.WriteLine("Using SQL Server provider");
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        return services;
    }
}
