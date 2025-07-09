
using MakerSchedule.Domain.Entities;

using Microsoft.EntityFrameworkCore;


namespace MakerSchedule.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<User> Users { get; }
    DbSet<Employee> Employees { get; }
    DbSet<Event> Events { get; }
    DbSet<Occurrence> Occurrences { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
