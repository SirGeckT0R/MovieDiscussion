using FluentValidation;
using MovieServiceApplication.Enums;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.ManageMovieInWatchlistCommand
{
    public class ManageMovieInWatchlistValidator : AbstractValidator<ManageMovieInWatchlistCommand>
    {
        public ManageMovieInWatchlistValidator()
        {
            RuleFor(command => command.AccountId)
                .NotEmpty();

            RuleFor(command => command.MovieId)
                .NotEmpty();

            RuleFor(x => x.Action)
                .NotEmpty()
                .Must(x => Enum.IsDefined(typeof(WatchlistAction), x));
        }
    }
}
