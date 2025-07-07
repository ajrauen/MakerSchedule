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

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Check if connection string contains SQL Server indicators
            if (connectionString?.Contains("Server=") == true || 
                connectionString?.Contains("Data Source=") == true && connectionString.Contains("Initial Catalog="))
            {
                // Use SQL Server for Azure
                options.UseSqlServer(connectionString);
            }
            else
            {
                // Use SQLite for local development
                options.UseSqlite(connectionString);
            }
        });

        return services;
    }
}
