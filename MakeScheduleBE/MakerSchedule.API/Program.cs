using MakerSchedule.API.Extensions;
using MakerSchedule.Application.Mappings;
using MakerSchedule.Application.Services;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MakerSchedule.API.Exceptions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddControllersWithErrorParser();

services.AddCorsWithOptions();
// Add Database
services.AddDatabase(configuration);

// Add Identity service with role support for User
services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.AddApplicationServices();

services.AddScoped<JwtService>();

// Add Database Seeder
services.AddScoped<DatabaseSeeder>();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured");
        var jwtIssuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured");
        var jwtAudience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience is not configured");
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});
// Add AutoMapper
services.AddAutoMapper(typeof(EmployeeMappingProfile));

// Add Swagger/OpenAPI
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MakerSchedule API", Version = "v1" });

    // Define the Bearer token security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Make sure Swagger UI requires a Bearer token to be specified
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

// Test database connection
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.CanConnectAsync();
    app.Logger.LogInformation("Successfully connected to the database.");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to connect to the database. Application will exit.");
    throw; // This will cause the application to exit
}

// Seed the database
try
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
    app.Logger.LogInformation("Database seeding completed successfully.");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to seed the database.");
    throw; // This will cause the application to exit
}

// Configure the HTTP request pipeline.
// Enable Swagger for both Development and Production
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MakerSchedule API v1"));

if (app.Environment.IsDevelopment())
{
    // Additional development-only configurations can go here
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"[DEBUG] Request received at (UTC): {DateTime.UtcNow:o}");
    await next.Invoke();
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

// Use the newer minimal API style routing
app.MapControllers();

app.UseExceptionHandler();

app.Run();
