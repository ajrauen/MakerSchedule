using MakerSchedule.Application.Events.Commands;
using MakerSchedule.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

using MediatR;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.SendEmail.Commands;

public class DeleteOccurrenceCommandHandler(IApplicationDbContext context, IMediator mediator) : IRequestHandler<DeleteOccurrenceCommand, bool>
{
    public async Task<bool> Handle(DeleteOccurrenceCommand request, CancellationToken cancellationToken)
    {
        var occurrence = await context.Occurrences.Include(o => o.Attendees).ThenInclude(a => a.User).Include(o => o.Event).FirstOrDefaultAsync(o => o.Id == request.OccurrenceId);
        if (occurrence == null)
            throw new NotFoundException($"Occurrence with id {request.OccurrenceId} not found", request.OccurrenceId);
        occurrence.isDeleted = true;
        context.Occurrences.Update(occurrence);

        await context.SaveChangesAsync();

        // Send cancellation emails in parallel without blocking the API response
        var emailTasks = occurrence.Attendees
            .Where(a => !string.IsNullOrEmpty(a.User.Email?.Value))
            .Select(attendee =>
            {
                var command = new SendClassCanceledEmailCommand(attendee.User.Email!.Value, new MakerSchedule.Application.Services.Email.Models.ClassCanceledEmailModel
                {
                    StudentName = attendee.User.FirstName,
                    EventName = occurrence.Event.EventName.Value,
                    ScheduleDate = occurrence.ScheduleStart?.Value.ToString("MMMM dd, yyyy") ?? "TBD",
                    ScheduleTime = occurrence.ScheduleStart?.Value.ToString("hh:mm tt") ?? "TBD",
                    ContactEmail = "andrewrauen@gmail.com",
                    ScheduleUrl = $"https://makerschedule.com/events/{occurrence.EventId}"
                });
                
                return Task.Run(async () => await mediator.Send(command));
            });

        _ = Task.WhenAll(emailTasks);

        return true;
    }
}