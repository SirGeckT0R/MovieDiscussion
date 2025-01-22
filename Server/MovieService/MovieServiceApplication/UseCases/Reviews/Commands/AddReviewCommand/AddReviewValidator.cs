using FluentValidation;

namespace MovieServiceApplication.UseCases.Reviews.Commands.AddReviewCommand
{
    public class AddReviewValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewValidator() 
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.MovieId)
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
