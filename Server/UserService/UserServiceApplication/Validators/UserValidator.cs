using FluentValidation;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .MaximumLength(100)
                .EmailAddress()
                .WithMessage("User's {PropertyName} is required");

            RuleFor(user => user.Username)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("User's {PropertyName} is required");

            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("User's {PropertyName} is required");
        }
    }
}
