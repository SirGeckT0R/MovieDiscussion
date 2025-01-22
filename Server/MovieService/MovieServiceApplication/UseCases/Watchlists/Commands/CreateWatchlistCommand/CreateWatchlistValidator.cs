using FluentValidation;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.CreateWatchlistCommand
{
    public class CreateWatchlistValidator : AbstractValidator<CreateWatchlistCommand>
    {
        public CreateWatchlistValidator() {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
