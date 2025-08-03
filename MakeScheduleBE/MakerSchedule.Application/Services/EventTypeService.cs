using MakerSchedule.Application.DTO.EventType;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.EventType;
using MakerSchedule.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class EventTypeService(ILogger<EventTypeService> logger, IApplicationDbContext context) : IEventTypeService
{
    public async Task<IEnumerable<EventTypeDTO>> GetAllEventTypesAsync()
    {
        var types = await context.EventTypes.Select(e => new EventTypeDTO
        {
            Id = e.Id,
            Name = e.Name.Value
        }).ToListAsync();

        return types;
    }

    public async Task<Guid> CreateEventTypeAsync(CreateEventTypeDTO eventTypeDTO)
    {
        var eventType = new EventType
        {
            Name = new EventTypeName(eventTypeDTO.Name)
        };

        context.EventTypes.Add(eventType);
        await context.SaveChangesAsync();

        return eventType.Id;
    }

    public async Task<bool> DeleteEventTypeAsync(Guid eventTypeId)
    {
        var eventType = await context.EventTypes.FindAsync(eventTypeId);
        if (eventType == null) return false;

        context.EventTypes.Remove(eventType);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> PatchEventTypeAsync(Guid eventTypeId, PatchEventTypeDTO eventTypeDTO)
    {
        var eventType = await context.EventTypes.FindAsync(eventTypeId);
        if (eventType == null) return false;

        if (eventTypeDTO.Name != null)
            eventType.Name = new EventTypeName(eventTypeDTO.Name);

        return await context.SaveChangesAsync() > 0;
    }

}