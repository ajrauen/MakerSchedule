namespace MakerSchedule.API.Extensions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services.EmailService;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Infrastructure.Services.Storage;
using MakerSchedule.API.Services;

public static class MakerScheduleExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(MakerScheduleExtensions).Assembly);

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();


        services.AddScoped<IImageStorageService>(serviceProvider =>
        {
            return new LocalImageStorageService(
                serviceProvider.GetRequiredService<IHostEnvironment>(),
                serviceProvider.GetRequiredService<IHttpContextAccessor>());


            // var env = serviceProvider.GetService<IWebHostEnvironment>();
            // return env?.IsDevelopment() == true
            // ? new LocalImageStorageService(
            //     serviceProvider.GetRequiredService<IHostEnvironment>(),
            //     serviceProvider.GetRequiredService<IHttpContextAccessor>())
            // : new AzureImageStorageService(serviceProvider.GetRequiredService<IConfiguration>());
        });

        
        // Register the database seeder
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}