using FluentValidation;

namespace MovieServiceApplication.UseCases.Reviews.Commands.UpdateReviewCommand
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Review's {PropertyName} is required");

            RuleFor(x => x.Value)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(10)
                .WithMessage("Review value must be between 1 and 10");

            RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Review's {PropertyName} is empty or too long");
        }
    }
}
