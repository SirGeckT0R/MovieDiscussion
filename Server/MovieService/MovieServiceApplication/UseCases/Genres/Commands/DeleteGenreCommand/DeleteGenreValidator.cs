using FluentValidation;

namespace MovieServiceApplication.UseCases.Genres.Commands.DeleteGenreCommand
{
    public class DeleteGenreValidator : AbstractValidator<DeleteGenreCommand>
    {
        public DeleteGenreValidator() {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Genre's {PropertyName} is required");
        }
    }
}
