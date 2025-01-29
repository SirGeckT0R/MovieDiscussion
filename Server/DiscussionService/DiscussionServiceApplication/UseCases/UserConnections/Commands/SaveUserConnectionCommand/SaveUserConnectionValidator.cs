using FluentValidation;

namespace DiscussionServiceApplication.UseCases.UserConnections.Commands.SaveUserConnectionCommand
{
    public class SaveUserConnectionValidator : AbstractValidator<SaveUserConnectionCommand>
    {
        public SaveUserConnectionValidator()
        {
            RuleFor(x => x.DiscussionId)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(x => x.AccountIdClaimValue)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(x => x.ConnectionId)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");
        }
    }
}
