
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace MakerSchedule.Infrastructure.Data;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.Customer;
using MakerSchedule.Domain.Aggregates.Employee;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Aggregates.User;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>, IApplicationDbContext
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

        modelBuilder.Entity<Customer>().HasData(SeedData.SeedCustomers.ToArray());
        modelBuilder.Entity<Event>().HasData(SeedData.SeedEvents.ToArray());
        modelBuilder.Entity<Occurrence>().HasData(SeedData.SeedOccurrences.ToArray());

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.EmployeeNumber)
            .IsUnique();

        // Configure Event entity to include EventType
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.EventType).IsRequired();
        });

        // Configure Occurrence entity to handle collections properly
        modelBuilder.Entity<Occurrence>(entity =>
        {
            // Configure Attendees collection to be stored as JSON
            entity.Property(e => e.Attendees)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<ICollection<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>())
                .Metadata.SetValueComparer(new ValueComparer<ICollection<int>>(
                    (c1, c2) => (c1 ?? Array.Empty<int>()).SequenceEqual(c2 ?? Array.Empty<int>()),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            // Configure Leaders collection to be stored as JSON
            entity.Property(e => e.Leaders)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<ICollection<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>())
                .Metadata.SetValueComparer(new ValueComparer<ICollection<int>>(
                    (c1, c2) => (c1 ?? Array.Empty<int>()).SequenceEqual(c2 ?? Array.Empty<int>()),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            // Ensure ScheduleStart is properly configured for SQL Server
            entity.Property(e => e.ScheduleStart)
                .HasColumnType("datetime2");
        });

        // Ensure unlimited string columns use TEXT for SQLite
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        property.SetColumnType("TEXT");
                    }
                }
            }
        }
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Occurrence> Occurrences { get; set; }
}
