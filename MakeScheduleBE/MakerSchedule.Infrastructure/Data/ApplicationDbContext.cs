
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace MakerSchedule.Infrastructure.Data;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.DomainUser;
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

        modelBuilder.Entity<Event>().HasData(SeedData.SeedEvents.ToArray());
        modelBuilder.Entity<Occurrence>().HasData(SeedData.SeedOccurrences.ToArray());

        // Configure Event entity to include EventType
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.EventType).IsRequired();
        });

        // Configure Occurrence entity
        modelBuilder.Entity<Occurrence>(entity =>
        {
            // Ensure ScheduleStart is properly configured for SQL Server
            entity.Property(e => e.ScheduleStart)
                .HasColumnType("datetime2");
        });

        // Configure many-to-many: Users lead Occurrences
        modelBuilder.Entity<OccurrenceLeader>(entity =>
        {
            entity.HasKey(ol => ol.Id);
            
            entity.HasOne(ol => ol.Occurrence)
                .WithMany(o => o.Leaders)
                .HasForeignKey(ol => ol.OccurrenceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ol => ol.User)
                .WithMany(u => u.OccurrencesLed)
                .HasForeignKey(ol => ol.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure many-to-many: Users attend Occurrences
        modelBuilder.Entity<OccurrenceAttendee>(entity =>
        {
            entity.HasKey(oa => oa.Id);
            
            entity.HasOne(oa => oa.Occurrence)
                .WithMany(o => o.Attendees)
                .HasForeignKey(oa => oa.OccurrenceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oa => oa.User)
                .WithMany(u => u.OccurrencesAttended)
                .HasForeignKey(oa => oa.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public DbSet<DomainUser> DomainUsers { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Occurrence> Occurrences { get; set; }
    public DbSet<OccurrenceLeader> OccurrenceLeaders { get; set; }
    public DbSet<OccurrenceAttendee> OccurrenceAttendees { get; set; }
}
