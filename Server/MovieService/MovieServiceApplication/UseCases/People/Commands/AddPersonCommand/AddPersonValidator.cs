using FluentValidation;

namespace MovieServiceApplication.UseCases.People.Commands.AddPersonCommand
{
    public class AddPersonValidator : AbstractValidator<AddPersonCommand>
    {
        public AddPersonValidator() {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Person's {PropertyName} is empty or too long");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Person's {PropertyName} is empty or too long");


            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .Must(x => x < DateTime.Now)
                .WithMessage("Person's {PropertyName} is required");
        }
    }
}
