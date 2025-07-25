using System.Text.Json;
using System.Text.Json.Serialization;

namespace MakerSchedule.API.Extensions;

public static class ControllerExtensions
{
    public static IMvcBuilder AddControllersWithErrorParser(this IServiceCollection services)
    {
        return services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .AddXmlDataContractSerializerFormatters();
    }

    public static IServiceCollection AddCorsWithOptions(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.WithOrigins("https://mango-water-01fdd2010.2.azurestaticapps.net", "http://localhost:5173", "https://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
    }
}

