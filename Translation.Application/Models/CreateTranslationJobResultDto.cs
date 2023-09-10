namespace Translation.Application.Models
{
    public class CreateTranslationJobResultDto
    {
        public string Id { get; set; }

        public CreateTranslationJobResultDto(string id)
        {
            Id = id;
        }
    }
}
