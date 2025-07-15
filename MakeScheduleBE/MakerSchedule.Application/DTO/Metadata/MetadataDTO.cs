namespace MakerSchedule.Application.DTO.Metadata;

public class MetadataDTO
{
    public Dictionary<int, string> Durations { get; set; } = new();
    public Dictionary<int, string> EventTypes {get; set;} = new();

}