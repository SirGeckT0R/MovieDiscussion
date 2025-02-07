using FluentValidation;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.UpdateDiscussionCommand
{
    public class UpdateDiscussionValidator : AbstractValidator<UpdateDiscussionCommand>
    {
        public UpdateDiscussionValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Discussion's {PropertyName} is required");

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Discussion's {PropertyName} is empty or too long");

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Discussion's {PropertyName} is empty or too long");

            RuleFor(x => x.UpdatedBy)
                .NotEmpty()
                .WithMessage("Discussion's {PropertyName} is required");
        }
    }
}
