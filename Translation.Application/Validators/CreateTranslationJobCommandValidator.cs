using FluentValidation;
using Translation.Application.Models;

namespace Translation.Application.Validators
{
    public class CreateTranslationJobCommandValidator : AbstractValidator<CreateTranslationJobCommand>
    {
        public CreateTranslationJobCommandValidator()
        {
            RuleFor(x => x.ToLanguage).NotEmpty();
            RuleFor(x => x.TextToTranslate).NotEmpty();
        }
    }
}
