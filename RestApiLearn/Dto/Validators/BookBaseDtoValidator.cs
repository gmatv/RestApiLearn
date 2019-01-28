using FluentValidation;

namespace RestApiLearn.Dto.Validators
{
    public class BookBaseDtoValidator : AbstractValidator<BookBaseDto>
    {
        public BookBaseDtoValidator()
        {
            RuleFor(b => b.Title).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Description).MaximumLength(500).NotEqual(b => b.Title).WithMessage("Title and Description can't be the same.");
        }
    }
}
