using Azure.Data.Tables;
using System.Text.Json;
using Translation.Application.Models;
using Translation.Domain.Models;
using Translation.Service.BlobStorage;
using Translation.Service.TableStorage;

namespace Translation.Application.Queries
{
    public class ReadTranslationQuery
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly ITableStorageService _tableStorageService;

        public ReadTranslationQuery(IBlobStorageService blobStorageService, ITableStorageService tableStorageService)
        {
            _blobStorageService = blobStorageService;
            _tableStorageService = tableStorageService;
        }

        public async Task<TranslatedTextResponse> Execute(string id)
        {
            TranslatedTextResponse translatedText = new();
            var translationTableEntity = await _tableStorageService.GetEntity(new TranslationEntityModel() { Id = id });

            if (translationTableEntity.RowKey == null)
            {
                return translatedText;
            }
            translatedText = MapTableEntityToTranslatedTextDto(translationTableEntity, translatedText);
            if (translatedText.Status == TranslationJobStatusEnum.Completed)
            {
                var translationBlobContent = await _blobStorageService.GetBlobContent(id);
                var translation = JsonSerializer.Deserialize<TranslatedTextModel>(translationBlobContent);
                if (translation != null)
                {
                    translatedText = MapTranslationToTranslatedTextDto(translation, translatedText);
                }
            }
            return translatedText;
        }

        private TranslatedTextResponse MapTableEntityToTranslatedTextDto(TableEntity translationTableEntity, TranslatedTextResponse? translatedText)
        {
            translationTableEntity.TryGetValue("Status", out object statusObject);
            Enum.TryParse(statusObject.ToString(), out TranslationJobStatusEnum status);
            translatedText.Id = translationTableEntity.RowKey;
            translatedText.Status = status;
            return translatedText;
        }

        private TranslatedTextResponse MapTranslationToTranslatedTextDto(TranslatedTextModel translation, TranslatedTextResponse? translatedText)
        {
            translatedText.SourceText = translation.SourceText;
            translatedText.TranslatedText = translation.TranslatedText;
            translatedText.DetectedLanguage = translation.DetectedLanguage;
            translatedText.ToLanguage = translation.ToLanguage;
            return translatedText;
        }
    }
}
