using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace MakerSchedule.Infrastructure.Data;
using MakerSchedule.Application.Interfaces;
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
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            // Configure Leaders collection to be stored as JSON
            entity.Property(e => e.Leaders)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<ICollection<int>>(v, (JsonSerializerOptions?)null) ?? new List<int>())
                .Metadata.SetValueComparer(new ValueComparer<ICollection<int>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            // Ensure ScheduleStart is properly configured for SQL Server
            entity.Property(e => e.ScheduleStart)
                .HasColumnType("datetime2");
        });
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Occurrence> Occurrences { get; set; }
}
