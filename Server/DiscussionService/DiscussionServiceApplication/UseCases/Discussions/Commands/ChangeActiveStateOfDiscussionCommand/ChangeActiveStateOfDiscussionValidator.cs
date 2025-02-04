using FluentValidation;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.ChangeActiveStateOfDiscussionCommand
{
    public class ChangeActiveStateOfDiscussionValidator : AbstractValidator<ChangeActiveStateOfDiscussionCommand>
    {
        public ChangeActiveStateOfDiscussionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(x => x.NewState)
                .NotNull()
                .WithMessage("{PropertyName} is required");
        }
    }
}
