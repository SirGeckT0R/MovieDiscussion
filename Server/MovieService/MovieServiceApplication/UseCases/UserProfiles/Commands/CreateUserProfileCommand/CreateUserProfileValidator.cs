using FluentValidation;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileValidator : AbstractValidator<CreateUserProfileCommand>
    {
        public CreateUserProfileValidator() 
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("Profile's {PropertyName} is required");

            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Profile's {PropertyName} is empty or too long");
        }
    }
}
