using Azure;
using Azure.AI.Translation.Text;
using Microsoft.Extensions.Configuration;
using Translation.Domain.Models;

namespace Translation.Service.TranslationService
{
    public class AzureTranslationService : IAzureTranslationService
    {

        public TextTranslationClient client { get; set; }

        public AzureTranslationService(IConfiguration configuration)
        {
            // Azure Translation Service
            AzureKeyCredential credential = new(configuration["translation:apiKey"]);
            client = new(credential);
        }

        public async Task<TranslatedTextItem> TranslateText(TranslationJobModel translationJob)
        {
            var result = await client.TranslateAsync(translationJob.TextToTranslate, translationJob.ToLanguage);
            return result.Value.FirstOrDefault();
        }
    }
}
