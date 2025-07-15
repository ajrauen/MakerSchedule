using MakerSchedule.Application.DTO.Metadata;

namespace MakerSchedule.Application.Interfaces;

public interface IMetadataService
{
    Task<MetadataDTO> GetApplicationMetadata();
}

