using FluentValidation;

namespace MovieServiceApplication.UseCases.People.Commands.AddPersonCommand
{
    public class AddPersonValidator : AbstractValidator<AddPersonCommand>
    {
        public AddPersonValidator() {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);


            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .Must(x => x < DateTime.Now);
        }
    }
}
