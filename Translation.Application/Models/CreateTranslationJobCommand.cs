using MediatR;

namespace Translation.Application.Models
{
    public class CreateTranslationJobCommand : IRequest<CreateTranslationJobResponse>
    {
        public string ToLanguage { get; set; }
        public string TextToTranslate { get; set; }
    }
}
