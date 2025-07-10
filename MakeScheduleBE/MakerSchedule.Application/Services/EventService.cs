using System.ComponentModel.Design;

using MakerSchedule.Application.DTOs.Event;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class EventService : IEventService
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<EventService> _logger;
    private readonly IImageStorageService _imageStorageService;

    public EventService(IApplicationDbContext context, ILogger<EventService> logger, IImageStorageService imageStorageService)
    {
        _context = context;
        _logger = logger;
        _imageStorageService = imageStorageService;
    }

    public async Task<IEnumerable<EventListDTO>> GetAllEventsAsync()
    {
        return await _context.Events.Select(e => new EventListDTO
        {
            Id = e.Id,
            EventName = e.EventName,
            Description = e.Description,
            EventType = e.EventType,
            Duration = e.Duration,
            FileUrl = e.FileUrl,
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
            Duration = e.Duration,
            FileUrl = e.FileUrl,
        };
    }

    public async Task<int> CreateEventAsync(CreateEventDTO dto)
    {

        if (dto.FormFile == null || dto.FormFile.Length == 0)
        {
        throw new ArgumentException("Image file is required for event creation", nameof(dto.FormFile));
        }


      

        var e = new Event
        {
            EventName = dto.EventName,
            Description = dto.Description,
            EventType = dto.EventType,
            Duration = dto.Duration,
            
        };
        _context.Events.Add(e);
        await _context.SaveChangesAsync();


        string fileUrl;
        try
        {
            var fileName = $"{dto.EventName}_{e.Id}{Path.GetExtension(dto.FormFile.FileName)}";
            fileUrl = await _imageStorageService.SaveImageAsync(dto.FormFile, fileName);
            e.FileUrl = fileUrl;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Failed to save image for event {EventName}", dto.EventName);
            _context.Events.Remove(e);
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("Failed to save event image", ex);
        }

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