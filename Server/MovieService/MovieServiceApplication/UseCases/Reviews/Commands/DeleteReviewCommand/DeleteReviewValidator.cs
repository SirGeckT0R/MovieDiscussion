using FluentValidation;

namespace MovieServiceApplication.UseCases.Reviews.Commands.DeleteReviewCommand
{
    public class DeleteReviewValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Review's {PropertyName} is required");
        }
    }
}
