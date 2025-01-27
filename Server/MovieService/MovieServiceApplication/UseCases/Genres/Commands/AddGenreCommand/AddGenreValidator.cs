using FluentValidation;

namespace MovieServiceApplication.UseCases.Genres.Commands.AddGenreCommand
{
    public class AddGenreValidator : AbstractValidator<AddGenreCommand>
    {
        public AddGenreValidator() {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Genre's {PropertyName} is empty or too long");
        }
    }
}
