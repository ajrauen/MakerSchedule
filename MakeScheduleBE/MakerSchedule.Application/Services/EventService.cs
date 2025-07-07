using System.ComponentModel.Design;

using MakerSchedule.Application.DTOs.Event;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Domain.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EventService> _logger;

    public EventService(ApplicationDbContext context, ILogger<EventService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<EventListDTO>> GetAllEventsAsync()
    {
        return await _context.Events.Select(e => new EventListDTO
        {
            Id = e.Id,
            EventName = e.EventName,
            Description = e.Description,
            EventType = e.EventType,
            Duration = e.Duration
        }).ToListAsync();
    }

    public async Task<EventDTO> GetEventAsync(int id)
    {
        var e = await _context.Events.FindAsync(id);
        if (e == null) throw new NotFoundException("Event", id);
        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName,
            Description = e.Description,
            EventType = e.EventType,
            Duration = e.Duration
        };
    }

    public async Task<int> CreateEventAsync(CreateEventDTO dto)
    {
        var e = new Event
        {
            EventName = dto.EventName,
            Description = dto.Description,
            EventType = dto.EventType,
            Duration = dto.Duration
        };
        _context.Events.Add(e);
        await _context.SaveChangesAsync();
        return e.Id;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var e = await _context.Events.FindAsync(id);
        if (e == null) return false;
        _context.Events.Remove(e);
        await _context.SaveChangesAsync();
        return true;
    }
}