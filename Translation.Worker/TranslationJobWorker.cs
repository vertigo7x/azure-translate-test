using System.Text.Json;
using Translation.Domain.Models;
using Translation.Service.BlobStorage;
using Translation.Service.QueueStorage;
using Translation.Service.TableStorage;
using Translation.Service.TranslationService;

namespace TranslationWorker
{
    public class TranslationJobWorker : BackgroundService
    {
        private readonly ILogger<TranslationJobWorker> _logger;
        private readonly IQueueStorageService _queueStorageService;
        private readonly ITableStorageService _tableStorageService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAzureTranslationService _azureTranslationService;

        public TranslationJobWorker(
            ILogger<TranslationJobWorker> logger,
            IQueueStorageService queueStorageService,
            ITableStorageService tableStorageService,
            IBlobStorageService blobStorageService,
            IAzureTranslationService azureTranslationService
            )
        {
            _logger = logger;
            _queueStorageService = queueStorageService;
            _tableStorageService = tableStorageService;
            _blobStorageService = blobStorageService;
            _azureTranslationService = azureTranslationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var message = await _queueStorageService.ReceiveMessage();
                if (message != null)
                {
                    _logger.LogInformation($"Received message: {message?.MessageId}");
                    await _queueStorageService.DeleteMessage(message?.MessageId, message?.PopReceipt);
                    _logger.LogInformation($"Deleted message: {message.MessageId}");
                    try
                    {
                        TranslationJobModel translationJob = JsonSerializer.Deserialize<TranslationJobModel>(message?.MessageText);
                        _logger.LogInformation($"Sending Translation to Azure Translation: {translationJob.Id}");
                        //var translation = await _azureTranslationService.TranslateText(translationJob);
                        // Fake Translation
                        var translation = new TranslatedTextModel(
                            translationJob.Id,
                            translationJob.TextToTranslate,
                            translationJob.ToLanguage,
                            "en",
                            "Fake Translated Text"
                            );
                        _logger.LogInformation($"Translation received from Azure Translation Service: {translationJob.Id}");
                        _logger.LogInformation($"Sending Translation to Blob Storage: {translationJob.Id}");
                        await _blobStorageService.WriteBlobContent(translationJob.Id, JsonSerializer.Serialize(translation));
                        var translationEntity = new TranslationEntityModel();
                        translationEntity.Id = translationJob.Id;
                        _logger.LogInformation($"Updating Translation in Table Storage: {translationJob.Id}");
                        _tableStorageService.UpdateEntity(translationEntity);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error sending translation to Azure Translation: {message?.MessageId}");
                    }

                }
                else
                {
                    _logger.LogInformation($"No message received, waiting for 10 seconds!");
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}