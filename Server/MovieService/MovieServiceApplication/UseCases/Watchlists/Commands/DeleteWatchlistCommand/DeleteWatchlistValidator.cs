using FluentValidation;

namespace MovieServiceApplication.UseCases.Watchlists.Commands.DeleteWatchlistCommand
{
    public class DeleteWatchlistValidator : AbstractValidator<DeleteWatchlistCommand>
    {
        public DeleteWatchlistValidator() {
            RuleFor(x => x.AccountId)
                .NotEmpty();
        }
    }
}
