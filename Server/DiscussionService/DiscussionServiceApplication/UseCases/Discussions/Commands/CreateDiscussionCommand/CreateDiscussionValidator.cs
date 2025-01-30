using FluentValidation;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand
{
    public class CreateDiscussionValidator : AbstractValidator<CreateDiscussionCommand>
    {
        public CreateDiscussionValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Discussion's {PropertyName} is empty or too long");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Discussion's {PropertyName} is empty or too long");

            RuleFor(x => x.StartAt)
                .NotEmpty()
                .Must(x => x >= DateTime.UtcNow)
                .WithMessage("Discussion's {PropertyName} is required");


            RuleFor(x => x.CreatedBy)
                .NotEmpty()
                .WithMessage("Discussion's {PropertyName} is required");
        }
    }
}
