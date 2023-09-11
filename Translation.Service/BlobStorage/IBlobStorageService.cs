using Translation.Domain.Models;

namespace Translation.Service.BlobStorage
{
    public interface IBlobStorageService
    {
        public Task<string> GetBlobContent(string fileName);
        public Task WriteBlobContent(string fileName, string content);
    }
}
