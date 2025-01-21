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
                .EmailAddress();

            RuleFor(user => user.Username)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(user => user.Password)
                .NotEmpty();
        }
    }
}
