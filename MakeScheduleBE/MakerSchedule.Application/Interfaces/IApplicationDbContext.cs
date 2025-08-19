

using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace MakerSchedule.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<DomainUser> DomainUsers { get; }
    DbSet<Event> Events { get; }
    DbSet<Occurrence> Occurrences { get; }
    DbSet<OccurrenceLeader> OccurrenceLeaders { get; }
    DbSet<OccurrenceAttendee> OccurrenceAttendees { get; }
    DbSet<IdentityUserRole<Guid>> UserRoles { get; }
    DbSet<IdentityRole<Guid>> Roles { get; }
    DbSet<EventTag> EventTags { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
