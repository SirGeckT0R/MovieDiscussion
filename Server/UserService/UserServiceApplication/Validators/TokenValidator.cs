using FluentValidation;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Validators
{
    public class TokenValidator : AbstractValidator<Token>
    {
        public TokenValidator() {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.User)
                .NotEmpty();

            RuleFor(x => x.TokenType)
                .NotEmpty()
                .NotEqual(ETokenType.None);

            RuleFor(x => x.TokenValue)
                .NotEmpty();

            RuleFor(x => x.ExpiresAt)
                .NotEmpty();

            RuleFor(x => x.CreatedAt)
                .NotEmpty();
        }
    }
}
