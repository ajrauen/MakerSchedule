using MakerSchedule.Application.DTO.Metadata;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Enums;

using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class MetadataService(ILogger<MetadataService> logger): IMetadataService
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


    public Task<MetadataDTO> GetApplicationMetadata()
    {
        var eventTypeDict = Enum.GetValues<EventTypeEnum>().ToDictionary(e =>(int)e, e => e.ToString());

        var dto = new MetadataDTO
        {
            Durations = Durations,
            EventTypes = eventTypeDict
        };

        return  Task.FromResult(dto) ;
    }

}

