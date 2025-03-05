using FluentValidation;

namespace MovieServiceApplication.UseCases.Movies.Commands.ManageNotApprovedMovieCommand
{
    public class ManageNotApprovedMovieValidator : AbstractValidator<ManageNotApprovedMovieCommand>
    {
        public ManageNotApprovedMovieValidator()
        {
            RuleFor(command => command.MovieId)
                .NotEmpty()
                .WithMessage("Movie's {PropertyName} is required");

            RuleFor(command => command.ShouldApprove)
                .NotNull()
                .WithMessage("Movie's {PropertyName} is required");
        }
    }
}
