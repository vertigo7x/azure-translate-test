using FluentValidation;
using Translation.Application.Models;

namespace Translation.Application.Validators
{
    public class TranslationJobDtoValidator : AbstractValidator<TranslationJobDto>
    {
        public TranslationJobDtoValidator()
        {
            RuleFor(x => x.ToLanguage).NotEmpty();
            RuleFor(x => x.TextToTranslate).NotEmpty();
        }
    }
}
