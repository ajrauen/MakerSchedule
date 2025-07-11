using MakerSchedule.Application.Exceptions;
namespace MakerSchedule.Application.Exceptions;

public class InvalidImageAspectRatioException(string message) : BaseException(message)
{
}
