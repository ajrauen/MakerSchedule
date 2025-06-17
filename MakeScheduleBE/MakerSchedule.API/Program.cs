using Microsoft.OpenApi.Models;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Application.Services;
using MakerSchedule.API.Exceptions;
using MakerSchedule.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Application.Mappings;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithErrorParser();

builder.Services.AddCorsWithOptions();
// Add Database
builder.Services.AddDatabase(builder.Configuration);

// Add Application Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(EmployeeMappingProfile));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MakerSchedule API", Version = "v1" });
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MakerSchedule API v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

// Use the newer minimal API style routing
app.MapControllers();

app.UseExceptionHandler();

app.Run();
