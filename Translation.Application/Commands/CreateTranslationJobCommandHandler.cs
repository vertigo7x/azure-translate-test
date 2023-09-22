using MediatR;
using Translation.Application.Models;
using Translation.Domain.Models;
using Translation.Service.QueueStorage;
using Translation.Service.TableStorage;

namespace Translation.Application.Commands
{
    public class CreateTranslationJobCommandHandler : IRequestHandler<CreateTranslationJobCommand, CreateTranslationJobResponse>
    {

        private readonly ITableStorageService _tableStorageService;
        private readonly IQueueStorageService _queueStorageService;

        public CreateTranslationJobCommandHandler(
            ITableStorageService tableStorageService,
            IQueueStorageService queueStorageService)
        {
            _tableStorageService = tableStorageService;
            _queueStorageService = queueStorageService;
        }

        public async Task<CreateTranslationJobResponse> Handle(CreateTranslationJobCommand model, CancellationToken cancellationToken)
        {
            TranslationEntityModel translationEntity = new();
            await _tableStorageService.InsertEntity(translationEntity);
            TranslationJobModel translationJobModel = new(translationEntity.Id, model.TextToTranslate, model.ToLanguage);
            await _queueStorageService.SendMessage(translationJobModel);
            return new CreateTranslationJobResponse(translationEntity.Id);
        }
    }
}
 