

using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Aggregates.User;

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
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
