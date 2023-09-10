using Translation.Application.Models;
using Translation.Domain.Models;
using Translation.Service.QueueStorage;
using Translation.Service.TableStorage;

namespace Translation.Application.Commands
{
    public class CreateTranslationJobCommand
    {

        private readonly ITableStorageService _tableStorageService;
        private readonly IQueueStorageService _queueStorageService;

        public CreateTranslationJobCommand(
            ITableStorageService tableStorageService,
            IQueueStorageService queueStorageService)
        {
            _tableStorageService = tableStorageService;
            _queueStorageService = queueStorageService;
        }

        public async Task<CreateTranslationJobResultDto> Handle(TranslationJobDto model)
        {
            // Insert Translation Job into Table Storage
            TranslationEntityModel translationEntity = new();
            await _tableStorageService.InsertEntity(translationEntity);
            // Send Translation Job to Queue
            TranslationJobModel translationJobModel = new(translationEntity.Id, model.TextToTranslate, model.ToLanguage);
            await _queueStorageService.SendMessage(translationJobModel);
            return new CreateTranslationJobResultDto(translationEntity.Id);
        }
    }
}
