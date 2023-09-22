namespace Translation.Application.Models
{
    public class CreateTranslationJobResponse
    {
        public string Id { get; set; }

        public CreateTranslationJobResponse(string id)
        {
            Id = id;
        }
    }
}
