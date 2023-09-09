using Translation.Domain.Models;

namespace Translation.Service.BlobStorage
{
    public interface IBlobStorageService
    {
        public Task<TranslatedTextModel> GetBlobContent(string fileName);
        public Task WriteBlobContent(string fileName, string content);
    }
}
