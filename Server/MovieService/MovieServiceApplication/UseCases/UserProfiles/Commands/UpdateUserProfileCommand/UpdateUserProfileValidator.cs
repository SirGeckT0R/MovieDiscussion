using FluentValidation;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.UpdateUserProfileCommand
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileValidator()
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
