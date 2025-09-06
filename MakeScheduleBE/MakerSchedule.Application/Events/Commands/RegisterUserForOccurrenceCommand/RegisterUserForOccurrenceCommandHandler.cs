

using MakerSchedule.Application.Events.Commands;
using MakerSchedule.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

using MediatR;

using Microsoft.Extensions.Logging;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Aggregates.Event;

public class RegisterUserForOccurrenceCommandHandler(IApplicationDbContext context, ILogger<RegisterUserForOccurrenceCommandHandler> logger, IImageStorageService imageStorageService) : IRequestHandler<RegisterUserForOccurrenceCommand, bool>
{
    public async Task<bool> Handle(RegisterUserForOccurrenceCommand request, CancellationToken cancellationToken)
    {
        var registerDTO = request.RegisterDTO;
        if (registerDTO == null)
            throw new ArgumentNullException(nameof(registerDTO));

        using var transaction = await ((DbContext)context).Database.BeginTransactionAsync();
        
        var occurrence = await context.Occurrences
            .Include(o => o.Attendees)
            .Include(o => o.Event)
            .FirstOrDefaultAsync(o => o.Id == registerDTO.OccurrenceId);
            
        if (occurrence == null)
        {
            throw new NotFoundException($"Occurrence with id {registerDTO.OccurrenceId} not found", registerDTO.OccurrenceId);
        }

        if (occurrence.ScheduleStart < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Cannot register for past occurrences");
        }

        if (occurrence.Attendees.Count >= occurrence.Event.ClassSize)
        {
            throw new InvalidOperationException($"Class is full. Maximum capacity is {occurrence.Event.ClassSize} attendees.");
        }

        var user = await context.DomainUsers.FirstOrDefaultAsync(du => du.Id == registerDTO.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with id {registerDTO.UserId} not found", registerDTO.UserId);
        }

        if (occurrence.Attendees.Any(a => a.UserId == registerDTO.UserId))
        {
            throw new InvalidOperationException($"User with id {registerDTO.UserId} is already registered for occurrence {registerDTO.OccurrenceId}");
        }

        var newAttendee = new OccurrenceAttendee
        {
            UserId = registerDTO.UserId,
            OccurrenceId = registerDTO.OccurrenceId,
            RegisteredAt = DateTime.UtcNow
        };

        context.OccurrenceAttendees.Add(newAttendee);

        var result = await context.SaveChangesAsync() > 0;
        await transaction.CommitAsync();
        return result;    }
}
