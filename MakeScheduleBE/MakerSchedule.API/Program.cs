using MakerSchedule.API.Extensions;
using MakerSchedule.Application.Services;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Application;

using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

using MakerSchedule.API.Exceptions;
using MakerSchedule.Domain.Aggregates.User;
// using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add Key Vault configuration for production only
// if (!builder.Environment.IsDevelopment())
// {
//     var keyVaultUrl = configuration["KeyVault:Url"];
//     if (!string.IsNullOrEmpty(keyVaultUrl))
//     {
//         configuration.AddAzureKeyVault(
//             new Uri(keyVaultUrl),
//             new DefaultAzureCredential());
//     }
// }

services.AddControllersWithErrorParser();

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplicationAssemblyMarker).Assembly));services.AddCorsWithOptions();

services.AddPostgreSqlDatabase(configuration);

services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.AddApplicationServices();

services.AddScoped<JwtService>();

services.AddAuthorizationWithPolicies(configuration);

services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MakerSchedule API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();

var app = builder.Build();

await app.EnsureDatabaseReadyAsync(); 


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MakerSchedule API v1"));


app.Use(async (context, next) =>
{
    Console.WriteLine($"[DEBUG] Request received at (UTC): {DateTime.UtcNow:o}");
    await next.Invoke();
});

// Only redirect to HTTPS in production if SSL is properly configured
if (!app.Environment.IsDevelopment() && app.Configuration["HTTPS_ENABLED"] == "true")
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles(); 
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
