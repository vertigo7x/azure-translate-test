using Azure.Storage.Queues.Models;
using Translation.Domain.Models;

namespace Translation.Service.QueueStorage
{
    public interface IQueueStorageService
    {
        public Task SendMessage(TranslationJobModel translationJob);
        public Task<QueueMessage> ReceiveMessage();
        public Task DeleteMessage(string messageId, string popReceipt);
    }
}
