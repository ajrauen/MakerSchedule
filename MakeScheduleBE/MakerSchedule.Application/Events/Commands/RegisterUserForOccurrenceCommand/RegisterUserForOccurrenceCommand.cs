
using MakerSchedule.Application.DTO.Occurrence;

using MediatR;

namespace MakerSchedule.Application.Events.Commands;


public class RegisterUserForOccurrenceCommand : IRequest<bool>
{
    public RegisterUserOccurrenceDTO RegisterDTO { get; }

    public RegisterUserForOccurrenceCommand(RegisterUserOccurrenceDTO registerDTO)
    {
        RegisterDTO = registerDTO;
    }
}