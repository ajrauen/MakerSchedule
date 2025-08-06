using MakerSchedule.API.Extensions;
using MakerSchedule.Application.Services;
using MakerSchedule.Infrastructure.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MakerSchedule.API.Exceptions;
using MakerSchedule.Domain.Aggregates.User;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllersWithErrorParser();

services.AddCorsWithOptions();
services.AddDatabase(configuration);

services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.AddApplicationServices();

services.AddScoped<JwtService>();

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

app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
