using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MakerSchedule.Domain.Entities;

namespace MakerSchedule.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<Employee>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial employee data
        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = "11111111-1111-1111-1111-111111111111",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                Address = "123 Main St",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                UserName = "john.doe@example.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            }
        );
    }

    public DbSet<Employee> Employees { get; set; }
} 