using FluentValidation;

namespace RestApiLearn.Dto.Validators
{
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator()
        {
            Include(new BookBaseDtoValidator());
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
