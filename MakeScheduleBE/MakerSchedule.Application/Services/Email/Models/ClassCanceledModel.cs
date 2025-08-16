namespace MakerSchedule.Application.Services.Email.Models;

public class ClassCanceledEmailModel
{
    public string StudentName { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string ScheduleDate { get; set; } = string.Empty;
    public string ScheduleTime { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ScheduleUrl { get; set; } = string.Empty;
}