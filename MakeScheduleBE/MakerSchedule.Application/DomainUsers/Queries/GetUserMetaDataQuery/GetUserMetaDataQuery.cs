using MakerSchedule.Application.DTO.Metadata;

using MediatR;

namespace MakerSchedule.Application.DomainUsers.Queries;

public class GetUserMetaDataQuery : IRequest<UserMetaDataDTO>;