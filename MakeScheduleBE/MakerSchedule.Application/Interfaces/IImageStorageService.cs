using Microsoft.AspNetCore.Http;

namespace MakerSchedule.Application.Interfaces;

public interface IImageStorageService
{
    Task<string> SaveImageAsync(IFormFile file, string fileName);
    Task<bool> DeleteImageAsync(string thumbnailUrl);
}