using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace FacebookLike.Service.GoogleCloud;

public class StorageService
{
    private readonly string _bucket;
    private readonly StorageClient _client;

    public StorageService(IConfiguration cfg)
    {
        _bucket = cfg["Gcp:BucketName"]!;
        
        if (string.IsNullOrEmpty(_bucket))
        {
            throw new ArgumentException("The Gcp:BucketName configuration is required.");
        }
        
        var credPath = cfg["Gcp:ServiceAccountPath"];
        var cred = GoogleCredential.FromFile(credPath);
        _client = StorageClient.Create(cred);
    }

    public async Task<string> UploadAndGetUrlAsync(Stream file, string originalName)
    {
        var objectName = $"{Guid.NewGuid()}{Path.GetExtension(originalName)}";
        var contentType = GetContentType(originalName);
        var obj = await _client.UploadObjectAsync(
            _bucket, objectName, contentType, file);

        return $"https://storage.googleapis.com/{_bucket}/{Uri.EscapeDataString(objectName)}";
    }

    private string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}