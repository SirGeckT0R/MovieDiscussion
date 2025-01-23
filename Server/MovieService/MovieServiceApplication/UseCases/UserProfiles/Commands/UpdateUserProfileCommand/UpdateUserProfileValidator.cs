using FluentValidation;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.UpdateUserProfileCommand
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty();

            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
