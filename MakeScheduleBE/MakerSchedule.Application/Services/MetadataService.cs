using MakerSchedule.Application.DTO.Metadata;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Constants;
using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.Services;

public class MetadataService() : IMetadataService
{
    private static readonly Dictionary<int, string> Durations = new()
    {
        {15 * 60 * 1000, "15 minutes" },
        {30 * 60 * 1000, "30 minutes" },
        {45 * 60 * 1000, "45 minutes" },
        {60 * 60 * 1000, "1 hour" },
        {90 * 60 * 1000, "1 hour 30 minutes"},
        {120 * 60 * 1000, "2 hours" },
        {150 * 60 * 1000, "2 hours 30 minutes" },
        {180 * 60 * 1000, "3 hours" },
        {210 * 60 * 1000, "3 hours 30 minutes" },
        {240 * 60 * 1000, "4 hours" },
    };

    public Task<EventsMetadataDTO> GetEventsMetadata()
    {
        var eventTypeArray = Enum.GetValues<EventTypeEnum>()
            .Cast<EventTypeEnum>()
            .Where(e => e != EventTypeEnum.Unknown)
            .Select(e => e.ToString())
            .ToArray();

        var dto = new EventsMetadataDTO
        {
            Durations = Durations,
            EventTypes = eventTypeArray
        };

        return Task.FromResult(dto);
    }

    public Task<UserMetaDataDTO> GetUsersMetadata()
    {
        var userMetadate = new UserMetaDataDTO
        {
            Roles = [
                Roles.Admin,
                Roles.Leader,
                Roles.Customer
            ]
        };

        return Task.FromResult(userMetadate);
    }
}

