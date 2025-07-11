using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MakerSchedule.Application.Interfaces;
using System.IO;

namespace MakerSchedule.Infrastructure.Services.Storage;

public class LocalImageStorageService(IHostEnvironment _env, IHttpContextAccessor _httpContextAccessor) : IImageStorageService
{
    public async Task<string> SaveImageAsync(IFormFile file, string fileName)
    {
        var wwwrootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
        var uploads = Path.Combine(wwwrootPath, "images", "events");
        Directory.CreateDirectory(uploads);
        var filePath = Path.Combine(uploads, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "http://localhost:5000";
        return $"{baseUrl}/images/events/{fileName}";
    }
}