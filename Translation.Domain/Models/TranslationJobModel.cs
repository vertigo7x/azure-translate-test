namespace Translation.Domain.Models
{
    public class TranslationJobModel : BaseModel
    {
        public string TextToTranslate { get; set; }
        public string ToLanguage { get; set; }

        public TranslationJobModel(string id, string textToTranslate, string toLanguage)
        {
            Id = id;
            TextToTranslate = textToTranslate;
            ToLanguage = toLanguage;
        }
    }
}
