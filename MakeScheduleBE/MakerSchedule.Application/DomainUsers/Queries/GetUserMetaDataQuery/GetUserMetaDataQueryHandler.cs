using MakerSchedule.Application.DomainUsers.Queries;
using MakerSchedule.Application.DTO.Metadata;
using MakerSchedule.Domain.Constants;

using MediatR;

public class GetUserMetaDataQueryHandler : IRequestHandler<GetUserMetaDataQuery, UserMetaDataDTO>
{
    public Task<UserMetaDataDTO> Handle(GetUserMetaDataQuery request, CancellationToken cancellationToken)
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