using FluentValidation;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Validators
{
    public class TokenValidator : AbstractValidator<Token>
    {
        public TokenValidator() {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Token's {PropertyName} is required");

            RuleFor(x => x.User)
                .NotEmpty()
                .WithMessage("Token's {PropertyName} is required");

            RuleFor(x => x.TokenType)
                .NotEmpty()
                .NotEqual(TokenType.None)
                .WithMessage("Token's {PropertyName} is required");

            RuleFor(x => x.TokenValue)
                .NotEmpty()
                .WithMessage("Token's {PropertyName} is required");

            RuleFor(x => x.ExpiresAt)
                .NotEmpty()
                .WithMessage("Token's {PropertyName} is required");

            RuleFor(x => x.CreatedAt)
                .NotEmpty()
                .WithMessage("Token's {PropertyName} is required");
        }
    }
}
