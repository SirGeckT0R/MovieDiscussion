using FluentValidation;

namespace MovieServiceApplication.UseCases.People.Commands.DeletePersonCommand
{
    public class DeletePersonValidator : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonValidator() {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Person's {PropertyName} is required");
        }
    }
}
