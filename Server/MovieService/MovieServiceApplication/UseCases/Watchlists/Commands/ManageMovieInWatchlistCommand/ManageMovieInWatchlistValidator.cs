using FluentValidation;
using MovieServiceApplication.Enums;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.ManageMovieInWatchlistCommand
{
    public class ManageMovieInWatchlistValidator : AbstractValidator<ManageMovieInWatchlistCommand>
    {
        public ManageMovieInWatchlistValidator()
        {
            RuleFor(command => command.AccountId)
                .NotEmpty()
                .WithMessage("Watchlist's {PropertyName} is required");

            RuleFor(command => command.MovieId)
                .NotEmpty()
                .WithMessage("Watchlist's {PropertyName} is required");

            RuleFor(x => x.Action)
                .NotEmpty()
                .Must(x => Enum.IsDefined(typeof(WatchlistAction), x))
                .WithMessage("Watchlist's {PropertyName} is required");
        }
    }
}
