using System.Threading.Tasks;

using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.SendEmail.Commands;
using MakerSchedule.Application.Services.Email.Models;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Domain.Exceptions;
using MakerSchedule.Domain.Utilties.ImageUtilities;
using MakerSchedule.Domain.ValueObjects;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class EventService(IApplicationDbContext context, ILogger<EventService> logger, IImageStorageService imageStorageService, IMediator mediator) : IEventService
{
    private readonly IApplicationDbContext _context = context;
    private readonly ILogger<EventService> _logger = logger;
    private readonly IImageStorageService _imageStorageService = imageStorageService;
    private const double RequiredAspectRatio = 4.0 / 3.0;



    public async Task<IEnumerable<EventListDTO>> GetAllEventsAsync()
    {
        return await _context.Events
            .Select(e => new EventListDTO
            {
                Id = e.Id,
                EventName = e.EventName.ToString(),
                Description = e.Description,
                EventTagIds = e.EventTagIds.ToArray(),
                Duration = e.Duration,
                ThumbnailUrl = e.ThumbnailUrl,
                ClassSize = e.ClassSize,
                Price = e.Price
            }).ToListAsync();
    }

    public async Task<EventDTO> GetEventAsync(Guid id)
    {
        var e = await _context.Events
        .Include(ev => ev.Occurrences)
            .ThenInclude(ev => ev.Attendees)
                .ThenInclude(a => a.User)
                    .ThenInclude(u => u.User)
        .Include(ev => ev.Occurrences)
            .ThenInclude(ev => ev.Leaders)
                .ThenInclude(l => l.User)
                    .ThenInclude(u => u.User)
        .FirstOrDefaultAsync(ev => ev.Id == id);

        if (e == null) throw new NotFoundException("Event", id);



        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,

            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
            EventTagIds = e.EventTagIds.ToArray(),
            ClassSize = e.ClassSize,
            Price = e.Price,
            occurrences = e.Occurrences
                .Where(o => !o.isDeleted)
                .Select(o => new OccurrenceDTO
                {
                    Id = o.Id,
                    Attendees = o.Attendees.Select(a => new OccurrenceAttendeeDTO
                    {
                        Id = a.UserId,
                        FirstName = a.User?.FirstName ?? "",
                        LastName = a.User?.LastName ?? "",
                        Email = a.User?.Email?.Value ?? ""
                    }).ToList(),
                    EventId = o.EventId,
                    Leaders = o.Leaders.Select(l => new OccurrenceAttendeeDTO
                    {
                        Id = l.UserId,
                        FirstName = l.User?.FirstName ?? "",
                        LastName = l.User?.LastName ?? "",
                        Email = l.User?.Email?.Value ?? ""
                    }).ToList(),
                    ScheduleStart = DateTime.SpecifyKind(o.ScheduleStart.Value, DateTimeKind.Utc),
                    Status = o.Status,
                    EventName = o.Event.EventName.Value,
                })
        };
    }

    public async Task<EventDTO> CreateEventAsync(CreateEventDTO dto)
    {

        if (dto.FormFile == null || dto.FormFile.Length == 0)
        {
            throw new ArgumentException("Image file is required for event creation", nameof(dto.FormFile));
        }


        var e = new Event
        {
            EventName = new EventName(dto.EventName),
            Description = dto.Description,
            Duration = dto.Duration > 0 ? new Duration(dto.Duration) : null,
            ClassSize = dto.ClassSize,
            EventTagIds = dto.EventTagIds?.ToList() ?? new List<Guid>(),
            Price = dto.Price
        };
        e.EventTagIds = dto.EventTagIds?.ToList() ?? new List<Guid>();
        _context.Events.Add(e);
        await _context.SaveChangesAsync();


        string thumbnailUrl;
        try
        {

            using (var stream = dto.FormFile.OpenReadStream())
            {
                if (ImageUtilities.IsSvg(stream))
                {
                    if (!ImageUtilities.IsSvgAspectRatioValid(stream, RequiredAspectRatio))
                    {
                        throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                    }
                }
                else if (!ImageUtilities.IsEventImageAspectRatioValid(stream, RequiredAspectRatio))
                {
                    throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                }
            }

            var fileName = $"{dto.EventName}_{e.Id}{Path.GetExtension(dto.FormFile.FileName)}";
            thumbnailUrl = await _imageStorageService.SaveImageAsync(dto.FormFile, fileName);
            e.ThumbnailUrl = thumbnailUrl;
            await _context.SaveChangesAsync();
        }
        catch (InvalidImageAspectRatioException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save image for event {EventName}", dto.EventName);
            _context.Events.Remove(e);
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("Failed to save event image", ex);
        }

        // Fetch EventTag data for the return DTO
        var eventTags = await _context.EventTags
            .Where(et => e.EventTagIds.Contains(et.Id))
            .Select(et => new EventTagDTO
            {
                Id = et.Id,
                Name = et.Name.Value
            })
            .ToListAsync();

        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,
            EventTagIds = eventTags.Select(et => et.Id).ToArray(),
            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
            ClassSize = e.ClassSize,
            Price = e.Price,
            occurrences = new List<OccurrenceDTO>()
        };
    }


    public async Task<EventDTO> PatchEventAsync(Guid eventId, PatchEventDTO dto)
    {
        var e = await _context.Events
            .Include(ev => ev.Occurrences)
                .ThenInclude(o => o.Attendees)
                    .ThenInclude(a => a.User)
            .Include(ev => ev.Occurrences)
                .ThenInclude(o => o.Leaders)
                    .ThenInclude(l => l.User)
            .FirstOrDefaultAsync(ev => ev.Id == eventId);
        if (e == null)
        {
            throw new NotFoundException($"Event with id {eventId} not found", eventId);
        }

        if (dto.EventName != null)
            e.EventName = new EventName(dto.EventName);
        if (dto.Description != null)
            e.Description = dto.Description;
        if (dto.Duration.HasValue)
            e.Duration = new Duration(dto.Duration.Value);

        if (dto.EventTagIds != null)
        {
            e.EventTagIds = dto.EventTagIds.ToList();
        }
        else
        {
            e.EventTagIds = new List<Guid>();
        }

        if (dto.ClassSize.HasValue)
            e.ClassSize = dto.ClassSize.Value;

        if (dto.Price.HasValue)
            e.Price = dto.Price.Value;

        if (dto.FormFile != null && dto.FormFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(e.ThumbnailUrl))
            {
                try
                {
                    await _imageStorageService.DeleteImageAsync(e.ThumbnailUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete previous image for event {EventName}", e.EventName);
                }
            }

            string thumbnailUrl;
            try
            {
                using (var stream = dto.FormFile.OpenReadStream())
                {
                    if (ImageUtilities.IsSvg(stream))
                    {
                        if (!ImageUtilities.IsSvgAspectRatioValid(stream, RequiredAspectRatio))
                        {
                            throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                        }
                    }
                    else if (!ImageUtilities.IsEventImageAspectRatioValid(stream, RequiredAspectRatio))
                    {
                        throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                    }
                }
                var fileName = $"{e.EventName}_{e.Id}{Path.GetExtension(dto.FormFile.FileName)}";
                thumbnailUrl = await _imageStorageService.SaveImageAsync(dto.FormFile, fileName);
                e.ThumbnailUrl = thumbnailUrl;
            }
            catch (InvalidImageAspectRatioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save image for event {EventName}", e.EventName);
                throw new InvalidOperationException("Failed to save event image", ex);
            }
        }

        await _context.SaveChangesAsync();

        // Fetch EventTag data separately since we're now using EventTagIds
        var eventTags = await _context.EventTags
            .Where(et => e.EventTagIds.Contains(et.Id))
            .Select(et => new EventTagDTO
            {
                Id = et.Id,
                Name = et.Name.Value
            })
            .ToArrayAsync();

        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,

            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
            EventTagIds = eventTags.Select(et => et.Id).ToArray(),
            ClassSize = e.ClassSize,
            Price = e.Price,
            occurrences = e.Occurrences
                .Where(o => !o.isDeleted)
                .Select(o => new OccurrenceDTO
                {
                    Id = o.Id,
                    Attendees = o.Attendees.Select(a => new OccurrenceAttendeeDTO
                    {
                        Id = a.UserId,
                        FirstName = a.User?.FirstName ?? "",
                        LastName = a.User?.LastName ?? "",
                        Email = a.User?.Email?.Value ?? ""
                    }).ToList(),
                    EventId = o.EventId,
                    Leaders = o.Leaders.Select(l => new OccurrenceAttendeeDTO
                    {
                        Id = l.UserId,
                        FirstName = l.User?.FirstName ?? "",
                        LastName = l.User?.LastName ?? "",
                        Email = l.User?.Email?.Value ?? ""
                    }).ToList(),
                    ScheduleStart = DateTime.SpecifyKind(o.ScheduleStart?.Value ?? DateTime.MinValue, DateTimeKind.Utc),
                    Status = o.Status,
                    EventName = o.Event.EventName.Value,
                }).ToList()
        };

    }



    public async Task<bool> DeleteEventAsync(Guid id)
    {
        var e = await _context.Events.FindAsync(id);
        if (e == null) return false;
        _context.Events.Remove(e);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<OccurrenceDTO> CreateOccurrenceAsync(CreateOccurrenceDTO occurrenceDTO)
    {

        var eventEntity = await _context.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurrenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurrenceDTO.EventId} not found", occurrenceDTO.EventId);

        OccurrenceInfo info;
        Occurrence newOccurrence;
        try
        {
            var start = occurrenceDTO.ScheduleStart;
            if (start.Kind == DateTimeKind.Local)
                start = start.ToUniversalTime();
            else if (start.Kind == DateTimeKind.Unspecified)
                start = DateTime.SpecifyKind(start, DateTimeKind.Local).ToUniversalTime();
            // Now start is always UTC
            info = new OccurrenceInfo(start);
            newOccurrence = eventEntity.AddOccurrence(info);
            _context.Occurrences.Add(newOccurrence);
        }
        catch (ScheduleDateOutOfBoundsException ex)
        {
            _logger.LogError("Exception type: {Type}, message: {Message}", ex.GetType().FullName, ex.Message);
            throw new BaseException(ex.Message, "SCHEDULE_START_INVALID", 400);
        }


        await _context.SaveChangesAsync();

        var uniqueLeaderIds = occurrenceDTO.Leaders.Distinct().ToList();
        var leaderDTOs = new List<OccurrenceAttendeeDTO>();
        var attendeeDTOs = new List<OccurrenceAttendeeDTO>();

        var allUserIds = uniqueLeaderIds.Concat(occurrenceDTO.Attendees).Distinct().ToList();
        var users = await _context.DomainUsers
            .Where(u => allUserIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        foreach (var leaderId in uniqueLeaderIds)
        {
            if (users.TryGetValue(leaderId, out var leader))
            {
                var occurrenceLeader = new OccurrenceLeader
                {
                    OccurrenceId = newOccurrence.Id,
                    UserId = leaderId,
                    AssignedAt = DateTime.UtcNow
                };
                _context.OccurrenceLeaders.Add(occurrenceLeader);

                leaderDTOs.Add(new OccurrenceAttendeeDTO
                {
                    Id = leaderId,
                    FirstName = leader.FirstName ?? "",
                    LastName = leader.LastName ?? "",
                    Email = leader.Email?.Value ?? ""
                });
            }
        }

        foreach (var attendeeId in occurrenceDTO.Attendees)
        {
            if (users.TryGetValue(attendeeId, out var attendee))
            {
                var occurrenceAttendee = new OccurrenceAttendee
                {
                    OccurrenceId = newOccurrence.Id,
                    UserId = attendeeId,
                    RegisteredAt = DateTime.UtcNow
                };
                _context.OccurrenceAttendees.Add(occurrenceAttendee);

                attendeeDTOs.Add(new OccurrenceAttendeeDTO
                {
                    Id = attendeeId,
                    FirstName = attendee.FirstName ?? "",
                    LastName = attendee.LastName ?? "",
                    Email = attendee.Email?.Value ?? ""
                });
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully created occurrence with {OccurrenceId}", newOccurrence.Id);

        return new OccurrenceDTO
        {
            Id = newOccurrence.Id,
            EventId = newOccurrence.EventId,
            ScheduleStart = DateTime.SpecifyKind(newOccurrence.ScheduleStart?.Value ?? DateTime.MinValue, DateTimeKind.Utc),
            Status = newOccurrence.Status,
            Attendees = attendeeDTOs,
            Leaders = leaderDTOs,
            EventName = eventEntity.EventName.Value,
        };
    }

    public async Task<OccurrenceDTO> UpdateOccuranceAsync(UpdateOccurrenceDTO occurrenceDTO)
    {


        var eventEntity = await _context.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurrenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurrenceDTO.EventId} not found", occurrenceDTO.EventId);

        var occurrence = await _context.Occurrences.FirstOrDefaultAsync(o => o.Id == occurrenceDTO.Id && o.EventId == occurrenceDTO.EventId);
        if (occurrence == null)
            throw new NotFoundException($"Occurrence with id {occurrenceDTO.Id} not found", occurrenceDTO.Id);

        var start = occurrenceDTO.ScheduleStart;
        if (start.Kind == DateTimeKind.Local)
            start = start.ToUniversalTime();
        else if (start.Kind == DateTimeKind.Unspecified)
            start = DateTime.SpecifyKind(start, DateTimeKind.Local).ToUniversalTime();
        occurrence.ScheduleStart = ScheduleStart.Create(start);

        var existingLeaders = _context.OccurrenceLeaders.Where(l => l.OccurrenceId == occurrence.Id);
        _context.OccurrenceLeaders.RemoveRange(existingLeaders);
        var existingAttendees = _context.OccurrenceAttendees.Where(a => a.OccurrenceId == occurrence.Id);
        _context.OccurrenceAttendees.RemoveRange(existingAttendees);

        var uniqueLeaderIds = occurrenceDTO.Leaders.Distinct().ToList();
        var uniqueAttendeeIds = occurrenceDTO.Attendees.Distinct().ToList();

        var leaderDTOs = new List<OccurrenceAttendeeDTO>();
        var attendeeDTOs = new List<OccurrenceAttendeeDTO>();


        // Fetch all users at once to avoid multiple DB calls
        var allUserIds = uniqueLeaderIds.Concat(uniqueAttendeeIds).Distinct().ToList();
        var users = await _context.DomainUsers
            .Where(u => allUserIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        foreach (var leaderId in uniqueLeaderIds)
        {
            if (users.TryGetValue(leaderId, out var leader))
            {
                var occurrenceLeader = new OccurrenceLeader
                {
                    OccurrenceId = occurrence.Id,
                    UserId = leaderId,
                    AssignedAt = DateTime.UtcNow
                };
                leaderDTOs.Add(new OccurrenceAttendeeDTO
                {
                    Id = leaderId,
                    FirstName = leader.FirstName ?? "",
                    LastName = leader.LastName ?? "",
                    Email = leader.Email?.Value ?? ""
                });
                _context.OccurrenceLeaders.Add(occurrenceLeader);
            }
        }

        foreach (var attendeeId in uniqueAttendeeIds)
        {
            if (users.TryGetValue(attendeeId, out var attendee))
            {
                var occurrenceAttendee = new OccurrenceAttendee
                {
                    OccurrenceId = occurrence.Id,
                    UserId = attendeeId,
                    RegisteredAt = DateTime.UtcNow
                };
                attendeeDTOs.Add(new OccurrenceAttendeeDTO
                {
                    Id = attendeeId,
                    FirstName = attendee.FirstName ?? "",
                    LastName = attendee.LastName ?? "",
                    Email = attendee.Email?.Value ?? ""
                });
                _context.OccurrenceAttendees.Add(occurrenceAttendee);
            }
        }

        await _context.SaveChangesAsync();
        return new OccurrenceDTO
        {
            Attendees = attendeeDTOs,
            Leaders = leaderDTOs,
            Id = occurrence.Id,
            EventId = occurrence.EventId,
            ScheduleStart = DateTime.SpecifyKind(occurrence.ScheduleStart?.Value ?? DateTime.MinValue, DateTimeKind.Utc),
            Status = occurrence.Status,
            EventName = occurrence.Event.EventName.Value,
        };
    }

    public async Task<bool> DeleteOccuranceAsync(Guid occurrenceid)
    {
        var occurrence = await _context.Occurrences.Include(o => o.Attendees).ThenInclude(a => a.User).Include(o => o.Event).FirstOrDefaultAsync(o => o.Id == occurrenceid);
        if (occurrence == null)
            throw new NotFoundException($"Occurrence with id {occurrenceid} not found", occurrenceid);
        occurrence.isDeleted = true;
        _context.Occurrences.Update(occurrence);

        await _context.SaveChangesAsync();

        foreach (var attendee in occurrence.Attendees)
        {
            var command = new SendClassCanceledEmailCommand(attendee.User.Email?.Value ?? "", new ClassCanceledEmailModel
            {
                StudentName = attendee.User.FirstName,
                EventName = occurrence.Event.EventName.Value,
                ScheduleDate = occurrence.ScheduleStart.Value.ToString("MMMM dd, yyyy"),
                ScheduleTime = occurrence.ScheduleStart.Value.ToString("hh:mm tt"),
                ContactEmail = "andrewrauen@gmail.com",
                ScheduleUrl = $"https://makerschedule.com/events/{occurrence.EventId}"
            });
            await mediator.Send(command);
        }

        return true;
    }

    public async Task<IEnumerable<OccurrenceDTO>> GetOccurancesByDateAsync(SearchOccurrenceDTO searchDTO)
    {
        if (searchDTO.EndDate < searchDTO.StartDate)
            throw new ArgumentException("End date must be greater than or equal to start date", nameof(searchDTO.EndDate));
        if (searchDTO.StartDate == null)
            throw new ArgumentNullException("Start date cannot be null");

        if (searchDTO.EndDate == null)
            throw new ArgumentNullException("End date cannot be null");


        if (searchDTO.StartDate.Kind == DateTimeKind.Local)
            searchDTO.StartDate = searchDTO.StartDate.ToUniversalTime();
        else if (searchDTO.StartDate.Kind == DateTimeKind.Unspecified)
            searchDTO.StartDate = DateTime.SpecifyKind(searchDTO.StartDate, DateTimeKind.Local).ToUniversalTime();

        if (searchDTO.EndDate.Kind == DateTimeKind.Local)
            searchDTO.EndDate = searchDTO.EndDate.ToUniversalTime();
        else if (searchDTO.EndDate.Kind == DateTimeKind.Unspecified)
            searchDTO.EndDate = DateTime.SpecifyKind(searchDTO.EndDate, DateTimeKind.Local).ToUniversalTime();

        var occurrences = await _context.Occurrences
            .Include(o => o.Event)
            .Include(o => o.Attendees)
                .ThenInclude(a => a.User)
            .Where(o => o.ScheduleStart >= searchDTO.StartDate && o.ScheduleStart <= searchDTO.EndDate)
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

    public async Task<bool> RegisterUserForOccurrenceAsync(RegisterUserOccurrenceDTO registerDTO)
    {
        if (registerDTO == null)
            throw new ArgumentNullException(nameof(registerDTO));

        using var transaction = await ((DbContext)_context).Database.BeginTransactionAsync();
        
        var occurrence = await _context.Occurrences
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

        var user = await _context.DomainUsers.FirstOrDefaultAsync(du => du.Id == registerDTO.UserId);
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

        _context.OccurrenceAttendees.Add(newAttendee);

        var result = await _context.SaveChangesAsync() > 0;
        await transaction.CommitAsync();
        return result;   
    }

}