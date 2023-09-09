using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Translation.Domain.Models;

namespace Translation.Service.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(IConfiguration configuration, BlobServiceClient blobServiceClient, ILogger<BlobStorageService> logger)
        {
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(configuration["blobStorage:storageName"]);
            _logger = logger;
        }

        public async Task<TranslatedTextModel> GetBlobContent(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            _logger.LogInformation($"BlobService: Reading blob with name: {fileName}");
            var blobDownloadInfo = await blobClient.DownloadAsync();
            var content = blobDownloadInfo.Value.Content;
            using var reader = new StreamReader(content);
            var result = await reader.ReadToEndAsync();
            _logger.LogInformation($"BlobService: Blob content: {result}");
            return JsonSerializer.Deserialize<TranslatedTextModel>(result);
        }

        public async Task WriteBlobContent(string fileName, string content)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            _logger.LogInformation($"BlobService: Writing blob with name: {fileName}");
            await blobClient.UploadAsync(BinaryData.FromString(content), overwrite: true);
        }
    }
}
