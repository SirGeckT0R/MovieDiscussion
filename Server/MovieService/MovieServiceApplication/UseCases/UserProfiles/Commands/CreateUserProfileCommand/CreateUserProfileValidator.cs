using FluentValidation;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileValidator : AbstractValidator<CreateUserProfileCommand>
    {
        public CreateUserProfileValidator() 
        {
            RuleFor(x => x.AccountId)
                .NotEmpty();

            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
