namespace MakerSchedule.API.Extensions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;

public static class MakerScheduleExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerRegistrationService, CustomerRegistrationService>();
        services.AddScoped<IEmployeeRegistrationService, EmployeeRegistrationService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmployeeProfileService, EmployeeProfileService>();
        services.AddScoped<ICustomerProfileService, CustomerProfileService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IOccurrenceService, OccurrenceService>();
        return services;
    }


}