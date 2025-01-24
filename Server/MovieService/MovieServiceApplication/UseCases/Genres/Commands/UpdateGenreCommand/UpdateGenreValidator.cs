using FluentValidation;

namespace MovieServiceApplication.UseCases.Genres.Commands.UpdateGenreCommand
{
    public class UpdateGenreValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Genre's {PropertyName} is required");
        }
    }
}
