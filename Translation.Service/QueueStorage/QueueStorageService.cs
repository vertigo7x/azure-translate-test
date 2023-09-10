using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Translation.Domain.Models;

namespace Translation.Service.QueueStorage
{
    public class QueueStorageService : IQueueStorageService
    {
        private readonly QueueClient _queueClient;
        private readonly ILogger<QueueStorageService> _logger;

        public QueueStorageService(IConfiguration configuration, QueueServiceClient queueServiceClient, ILogger<QueueStorageService> logger)
        {
            _queueClient = queueServiceClient.GetQueueClient(configuration["queueStorage:queueName"]);
            _logger = logger;
        }

        public async Task SendMessage(TranslationJobModel translationJob)
        {
            var message = JsonSerializer.Serialize(translationJob);
            _logger.LogInformation($"QueueService: Sending message: {message}");
            await _queueClient.SendMessageAsync(message);
        }
        public async Task<QueueMessage> ReceiveMessage()
        {
            var response = await _queueClient.ReceiveMessagesAsync();
            _logger.LogInformation($"Received message: {response.Value.FirstOrDefault()?.MessageText}");
            var message = response.Value.FirstOrDefault();
            if (message == null)
                return null;
            return message;
        }

        public async Task DeleteMessage(string messageId, string popReceipt)
        {
            _logger.LogInformation($"Deleting message: {messageId}");
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }
    }
}
