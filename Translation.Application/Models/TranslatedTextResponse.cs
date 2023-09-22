using Translation.Domain.Models;

namespace Translation.Application.Models
{
    public class TranslatedTextResponse
    {
        public string Id { get; set; }
        public TranslationJobStatusEnum Status { get; set; }
        public string? SourceText { get; set; }
        public string? TranslatedText { get; set; }
        public string? DetectedLanguage { get; set; }
        public string? ToLanguage { get; set; }


        public TranslatedTextResponse()
        {
        }
        public TranslatedTextResponse(
            string id,
            TranslationJobStatusEnum status,
            string? sourceText = null,
            string? translatedText = null,
            string? detectedLanguage = null,
            string? toLanguage = null)
        {
            Id = id;
            Status = status;
            SourceText = sourceText;
            TranslatedText = translatedText;
            DetectedLanguage = detectedLanguage;
            ToLanguage = toLanguage;
        }


    }
}
