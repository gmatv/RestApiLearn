using FluentValidation;

namespace RestApiLearn.Dto.Validators
{
    public class CreateBookDtoValidator: AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            Include(new BookBaseDtoValidator());
        }
    }
}
