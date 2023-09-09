using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<TranslatedTextDto> Execute(string id)
        {
            TranslatedTextDto translatedText = new();
            // Get Translation Job from Table Storage
            var translationEntity = await _tableStorageService.GetEntity(id);
            if (translationEntity != null)
            {
                MapTableEntityToTranslatedTextDto(translationEntity, translatedText);
                if (translatedText.Status == TranslationJobStatusEnum.Completed)
                {
                    // Get Translation from Blob Storage
                    var translation = await _blobStorageService.GetBlobContent(id);
                    if (translation != null)
                    {
                        MapTranslationToTranslatedTextDto(translation, translatedText);
                    }
                }
            }
            return translatedText;
        }

        private static void MapTableEntityToTranslatedTextDto(TableEntity translationEntity, TranslatedTextDto? translatedText)
        {
            translationEntity.TryGetValue("Status", out object statusObject);
            Enum.TryParse(statusObject.ToString(), out TranslationJobStatusEnum status);
            translatedText.Id = translationEntity.RowKey;
            translatedText.Status = status;
        }

        private static void MapTranslationToTranslatedTextDto(TranslatedTextModel translation, TranslatedTextDto? translatedText)
        {
            translatedText.SourceText = translation.SourceText;
            translatedText.TranslatedText = translation.TranslatedText;
            translatedText.DetectedLanguage = translation.DetectedLanguage;
            translatedText.ToLanguage = translation.ToLanguage;
        }
    }
}
