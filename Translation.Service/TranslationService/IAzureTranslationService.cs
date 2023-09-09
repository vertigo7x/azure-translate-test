using Azure.AI.Translation.Text;
using Translation.Domain.Models;

namespace Translation.Service.TranslationService
{
    public interface IAzureTranslationService
    {
        public Task<TranslatedTextItem> TranslateText(TranslationJobModel translationJob);
    }
}
