using System.Runtime.InteropServices;

using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Entities;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MakerSchedule.Application.Services;

public class EventTagService(IApplicationDbContext context, ILogger<EventTagService> logger) : IEventTagService
{
    public async Task<EventTagDTO> CreateEventTagAsync(CreateEventTagDTO createEventTagDto)
    {
        var eventTag = new EventTag
        {
            Name = createEventTagDto.Name,
            Color = createEventTagDto.Color
        };

        context.EventTags.Add(eventTag);
        await context.SaveChangesAsync();

        logger.LogInformation($"Event tag created: {eventTag.Id}");

        return new EventTagDTO
        {
            Id = eventTag.Id,
            Name = eventTag.Name.Value,
            Color = eventTag.Color
        };
    }

    public async Task<IEnumerable<EventTagDTO>> GetEventTagAsync()
    {
        var eventTags = await context.EventTags.ToListAsync();

        return eventTags.Select(tag => new EventTagDTO
        {
            Id = tag.Id,
            Name = tag.Name.Value,
            Color = tag.Color
        });
    }

    public async Task<EventTagDTO> GetEventTagByIdAsync(Guid eventTagId)
    {
        var eventTag = await context.EventTags.FirstOrDefaultAsync(ev => ev.Id == eventTagId);

        if (eventTag == null)
        {
            logger.LogWarning($"Event tag not found: {eventTagId}");
            throw new KeyNotFoundException($"Event tag with ID {eventTagId} not found.");
        }
        return new EventTagDTO
        {
            Id = eventTag.Id,
            Name = eventTag.Name.Value,
            Color = eventTag.Color
        };

    }

    public async Task<EventTagDTO> PatchEventTagAsync(Guid eventTagId, PatchEventTagDTO eventTagDTO)
    {
        var eventTag = await context.EventTags.FirstOrDefaultAsync(ev => ev.Id == eventTagId);

        if (eventTag == null)
        {
            logger.LogWarning($"Event tag not found: {eventTagId}");
            throw new KeyNotFoundException($"Event tag with ID {eventTagId} not found.");
        }

        if (eventTagDTO.Name != null)
        {
            eventTag.Name = eventTagDTO.Name;
        }

        if (eventTagDTO.Color != null)
        {
            eventTag.Color = eventTagDTO.Color;
        }

        await context.SaveChangesAsync();

        return new EventTagDTO
        {
            Id = eventTag.Id,
            Name = eventTag.Name.Value,
            Color = eventTag.Color
        };
    }

    public async Task<bool> DeleteEventTagAsync(Guid eventTagId)
    {
        var eventTag = await context.EventTags.FirstOrDefaultAsync(ev => ev.Id == eventTagId);

        if (eventTag == null)
        {
            throw new KeyNotFoundException($"Event tag with ID {eventTagId} not found.");
        }

        context.EventTags.Remove(eventTag);
        await context.SaveChangesAsync();
        return true;
    }
}
