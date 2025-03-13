using Bogus;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Services;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class BaseTest
    {
        protected readonly Mock<IUserUnitOfWork> _unitOfWork;
        protected readonly Mock<IValidator<Token>> _validator;
        protected readonly Mock<IJwtProvider> _jwtProvider;
        protected readonly Mock<ILogger<TokenService>> _logger;

        protected readonly Faker<Token> _tokenFaker;

        public BaseTest()
        {
            _unitOfWork = new Mock<IUserUnitOfWork>();
            _jwtProvider = new Mock<IJwtProvider>();
            _validator = new Mock<IValidator<Token>>();
            _logger = new Mock<ILogger<TokenService>>();

            _tokenFaker = TokenRelatedFakers.GetTokenFaker();
        }
    }
}
