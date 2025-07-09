using Azure.Storage.Blobs;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MakerSchedule.Infrastructure.Services.Storage;

public class AzureImageStorageService : IImageStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _blobContainerName;

    public AzureImageStorageService(IConfiguration configuration)
    {
        // Use Key Vault for production (this service is only used in production)
        var keyVaultUrl = configuration["KeyVault:Url"] ?? throw new InvalidOperationException("Key Vault URL is not configured");
        var credential = new DefaultAzureCredential();
        var secretClient = new SecretClient(new Uri(keyVaultUrl), credential);
        
        var connectionStringSecret = secretClient.GetSecret("StorageConnectionString");
        var connectionString = connectionStringSecret.Value.Value ?? throw new InvalidOperationException("Storage connection string not found in Key Vault");
        
        _blobContainerName = configuration["AzureStorage:ContainerName"] ?? throw new InvalidOperationException("Azure event image container name not configured");
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> SaveImageAsync(IFormFile file, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);

        if (!containerClient.Exists())
        {
            await containerClient.CreateIfNotExistsAsync();
        }

        var imageBlob = containerClient.GetBlobClient($"eventImages/{fileName}");
        var stream = file.OpenReadStream();

        await imageBlob.UploadAsync(stream, overwrite: true);
        
        return imageBlob.Uri.ToString();
    }
}





