namespace MakerSchedule.API.Extensions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;
using MakerSchedule.Application.Services.EmailService;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Infrastructure.Services.Storage;

public static class MakerScheduleExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MakerScheduleExtensions).Assembly);

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<MetadataService, MetadataService>();
        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IImageStorageService>(serviceProvider =>
        {
            var env = serviceProvider.GetService<IWebHostEnvironment>();
            return env?.IsDevelopment() == true
            ? new LocalImageStorageService(
                serviceProvider.GetRequiredService<IHostEnvironment>(),
                serviceProvider.GetRequiredService<IHttpContextAccessor>())
            : new AzureImageStorageService(serviceProvider.GetRequiredService<IConfiguration>());
        });

        services.AddScoped<IDomainUserProfileService, DomainUserProfileService>();
        services.AddScoped<IDomainUserService, DomainUserService>();
        
        // Register the database seeder
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}