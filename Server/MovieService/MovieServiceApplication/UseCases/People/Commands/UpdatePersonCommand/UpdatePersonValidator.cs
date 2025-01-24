using FluentValidation;

namespace MovieServiceApplication.UseCases.People.Commands.UpdatePersonCommand
{
    public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonValidator() {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Person's {PropertyName} is required");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Person's {PropertyName} is required");


            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .Must(x => x < DateTime.Now)
                .WithMessage("Person's {PropertyName} is required");
        }
    }
}
