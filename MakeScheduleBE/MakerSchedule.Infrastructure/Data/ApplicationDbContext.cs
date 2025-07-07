using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MakerSchedule.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure AspNetUsers table to include UserType
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.UserType).IsRequired();
            // Remove HasData for users since we'll create them through UserManager
        });

        modelBuilder.Entity<Employee>().HasData(SeedData.SeedEmployees.ToArray());
        modelBuilder.Entity<Customer>().HasData(SeedData.SeedCustomers.ToArray());
        modelBuilder.Entity<Event>().HasData(SeedData.SeedEvents.ToArray());

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.EmployeeNumber)
            .IsUnique();

        // Configure Event entity to include EventType
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.EventType).IsRequired();
        });
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Event> Events { get; set; }
}
