namespace MakerSchedule.Application.DTO.EventTag;
public class EventTagDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public ICollection<Guid> EventIds { get; set; } = new List<Guid>();
}
