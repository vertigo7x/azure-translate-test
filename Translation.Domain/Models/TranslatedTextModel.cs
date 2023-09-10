namespace Translation.Domain.Models
{
    public class TranslatedTextModel : BaseModel
    {
        public string SourceText { get; set; }
        public string TranslatedText { get; set; }
        public string DetectedLanguage { get; set; }
        public string ToLanguage { get; set; }

        public TranslatedTextModel()
        {
        }
        public TranslatedTextModel(string id, string sourceText, string translatedText, string detectedLanguage, string toLanguage)
        {
            Id = id;
            SourceText = sourceText;
            TranslatedText = translatedText;
            DetectedLanguage = detectedLanguage;
            ToLanguage = toLanguage;
        }
    }
}
