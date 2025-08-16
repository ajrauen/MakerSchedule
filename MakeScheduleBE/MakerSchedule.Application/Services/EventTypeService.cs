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

    public async Task<EventTypeDTO> CreateEventTypeAsync(CreateEventTypeDTO eventTypeDTO)
    {
        if (string.IsNullOrWhiteSpace(eventTypeDTO.Name))
            throw new ArgumentException("Event type name cannot be empty", nameof(eventTypeDTO.Name));

        var eventType = new EventType
        {
            Name = new EventTypeName(eventTypeDTO.Name)
        };

        context.EventTypes.Add(eventType);
        await context.SaveChangesAsync();

        return new EventTypeDTO
        {
            Id = eventType.Id,
            Name = eventType.Name.Value
        };
    }

    public async Task<bool> DeleteEventTypeAsync(Guid eventTypeId)
    {
        var eventType = await context.EventTypes.FindAsync(eventTypeId);
        if (eventType == null) throw new InvalidDataException("Event type not found");

        context.EventTypes.Remove(eventType);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<EventTypeDTO> PatchEventTypeAsync(Guid eventTypeId, PatchEventTypeDTO eventTypeDTO)
    {

        if (eventTypeId == null) throw new ArgumentException("Event type not found", nameof(eventTypeId));


        var eventType = await context.EventTypes.FindAsync(eventTypeId);


        if (eventTypeDTO.Name != null)
            eventType.Name = new EventTypeName(eventTypeDTO.Name);



        await context.SaveChangesAsync();

        return new EventTypeDTO
        {
            Id = eventType.Id,
            Name = eventType.Name.Value
        };
    }

}