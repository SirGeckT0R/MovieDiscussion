using FluentValidation;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.DeleteDiscussionCommand
{
    public class DeleteDiscussionValidator : AbstractValidator<DeleteDiscussionCommand>
    {
        public DeleteDiscussionValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Discussion's {PropertyName} is required");
        }
    }
}
