namespace MakerSchedule.Application.Constants;

public sealed record EmailTemplateFileName(string Value)
{
    public static readonly EmailTemplateFileName ClassCanceled = new("class-canceled.html");
    public static readonly EmailTemplateFileName ClassRescheduled = new("class-rescheduled.html");
    public static readonly EmailTemplateFileName Welcome = new("register-welcome.html");
}