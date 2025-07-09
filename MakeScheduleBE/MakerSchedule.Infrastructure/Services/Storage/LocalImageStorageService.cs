using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MakerSchedule.Application.Interfaces;
using System.IO;

namespace MakerSchedule.Infrastructure.Services.Storage;

public class LocalImageStorageService : IImageStorageService
{
    private readonly IHostEnvironment _env;
    public LocalImageStorageService(IHostEnvironment env) => _env = env;

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
        return $"/images/events/{fileName}";
    }
}