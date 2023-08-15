using FluentValidation;

namespace EncurtadorUrl.Models.Validations
{
    public class UrlValidation : AbstractValidator<UrlModel>
    {
        public UrlValidation()
        {
            RuleFor(c => c.Url)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 500).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
