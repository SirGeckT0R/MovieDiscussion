using FluentValidation;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand
{
    public class CreateWatchlistValidator : AbstractValidator<CreateWatchlistCommand>
    {
        public CreateWatchlistValidator() {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("Watchlist's {PropertyName} is required");
        }
    }
}
