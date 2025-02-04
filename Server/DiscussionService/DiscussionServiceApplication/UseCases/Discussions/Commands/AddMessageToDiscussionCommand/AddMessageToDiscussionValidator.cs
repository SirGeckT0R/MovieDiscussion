using FluentValidation;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.AddMessageToDiscussionCommand
{
    public class AddMessageToDiscussionValidator : AbstractValidator<AddMessageToDiscussionCommand>
    {
        public AddMessageToDiscussionValidator()
        {
            RuleFor(x => x.ConnectionId)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");
        }
    }
}
