using FluentValidation;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.SubscribeToDiscussionCommand
{
    public class SubscribeToDiscussionValidator : AbstractValidator<SubscribeToDiscussionCommand>
    {
        public SubscribeToDiscussionValidator()
        {
            RuleFor(x => x.DiscussionId)
                .NotEmpty()
                .WithMessage("{{PropertyName} is required");

            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("{{PropertyName} is required");

        }
    }
}
