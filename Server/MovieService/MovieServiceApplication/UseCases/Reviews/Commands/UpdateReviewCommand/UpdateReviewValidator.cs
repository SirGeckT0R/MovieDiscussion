using FluentValidation;

namespace MovieServiceApplication.UseCases.Reviews.Commands.UpdateReviewCommand
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Value)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(10);

            RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
