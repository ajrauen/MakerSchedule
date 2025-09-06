using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Events.Queries;
using MakerSchedule.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

using MediatR;

public class GetOccurrencesByDateQueryHandler(IApplicationDbContext context) : IRequestHandler<GetOccurrencesByDateQuery, IEnumerable<OccurrenceDTO>>
{
    public async Task<IEnumerable<OccurrenceDTO>> Handle(GetOccurrencesByDateQuery request, CancellationToken cancellationToken)
    {
        var search = request.searchDTO;

        if (search == null)
        {
            throw new ArgumentNullException(nameof(search), "Search criteria cannot be null.");
        }

        if(search.StartDate == default || search.EndDate == default)
        {
            throw new ArgumentException("Start date and end date must be provided.");
        }

        if (search.EndDate < search.StartDate)
        {
            throw new ArgumentException("End date cannot be earlier than start date.");
        }
        
     if (search.StartDate.Kind == DateTimeKind.Local)
            search.StartDate = search.StartDate.ToUniversalTime();
        else if (search.StartDate.Kind == DateTimeKind.Unspecified)
            search.StartDate = DateTime.SpecifyKind(search.StartDate, DateTimeKind.Local).ToUniversalTime();

        if (search.EndDate.Kind == DateTimeKind.Local)
            search.EndDate = search.EndDate.ToUniversalTime();
        else if (search.EndDate.Kind == DateTimeKind.Unspecified)
            search.EndDate = DateTime.SpecifyKind(search.EndDate, DateTimeKind.Local).ToUniversalTime();

        var occurrences = await context.Occurrences
            .Include(o => o.Event)
            .Include(o => o.Attendees)
                .ThenInclude(a => a.User)
            .Where(o => o.ScheduleStart >= search.StartDate && o.ScheduleStart <= search.EndDate)
            .Where(o => !o.isDeleted)
            .Select(o => new OccurrenceDTO
            {
                Id = o.Id,
                EventId = o.EventId,
                ScheduleStart = DateTime.SpecifyKind(o.ScheduleStart.Value, DateTimeKind.Utc),
                Status = o.Status,
                EventName = o.Event.EventName.Value,
                Attendees = o.Attendees.Select(a => new OccurrenceAttendeeDTO
                {
                    Id = a.UserId,
                    FirstName = a.User.FirstName ?? "",
                    LastName = a.User.LastName ?? "",
                    Email = a.User.Email != null ? a.User.Email.Value : ""
                }).ToList(),
                Leaders = o.Leaders.Select(l => new OccurrenceAttendeeDTO
                {
                    Id = l.UserId,
                    FirstName = l.User.FirstName ?? "",
                    LastName = l.User.LastName ?? "",
                    Email = l.User.Email != null ? l.User.Email.Value : ""
                }).ToList()
            }).ToListAsync();

        return occurrences;
    }
}