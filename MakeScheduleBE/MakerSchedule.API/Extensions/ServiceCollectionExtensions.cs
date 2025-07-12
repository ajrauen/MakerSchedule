namespace MakerSchedule.API.Extensions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Infrastructure.Services.Storage;
using AutoMapper;

public static class MakerScheduleExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(typeof(MakerScheduleExtensions).Assembly);

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IOccurrenceService, OccurrenceService>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
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