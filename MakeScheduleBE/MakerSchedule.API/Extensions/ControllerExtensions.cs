using System.Text.Json;
using System.Text.Json.Serialization;
using MakerSchedule.API.Exceptions;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace MakerSchedule.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IMvcBuilder AddControllersWithErrorParser(this IServiceCollection services)
        {
            return services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext => ValidationErrorHandler.HandleValidationErrors(actionContext.ModelState);
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            })
            .AddXmlDataContractSerializerFormatters(); 
        }

        public static IServiceCollection AddCorsWithOptions(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });
        }

    }

} 

