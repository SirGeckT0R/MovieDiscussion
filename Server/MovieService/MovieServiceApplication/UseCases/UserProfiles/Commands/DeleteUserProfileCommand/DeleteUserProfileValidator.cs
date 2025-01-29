using FluentValidation;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.DeleteUserProfileCommand
{
    public class DeleteUserProfileValidator : AbstractValidator<DeleteUserProfileCommand>
    {
        public DeleteUserProfileValidator() 
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("Profile's {PropertyName} is required");
        }
    }
}
