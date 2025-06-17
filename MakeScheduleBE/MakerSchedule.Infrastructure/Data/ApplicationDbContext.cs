using Microsoft.EntityFrameworkCore;
using MakerSchedule.Domain.Entities;

namespace MakerSchedule.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Entity configurations will be added here
        // Seed initial employee data
        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890",
                Address = "123 Main St",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            }
        );
    }

    public DbSet<Employee> Employees { get; set; }
} 