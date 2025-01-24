using FluentValidation;

namespace MovieServiceApplication.UseCases.Movies.Commands.DeleteMovieCommand
{
    public class DeleteMovieValidator : AbstractValidator<DeleteMovieCommand>
    {
        public DeleteMovieValidator() {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Movie's {PropertyName} is required");
        }
    }
}
