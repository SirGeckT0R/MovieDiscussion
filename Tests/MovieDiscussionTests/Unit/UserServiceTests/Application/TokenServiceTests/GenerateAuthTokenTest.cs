using Bogus;
using Moq;
using UserServiceDataAccess.DatabaseHandlers.Specifications.TokenSpecifications;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Models;
using UserServiceDataAccess.Enums;
using UserServiceApplication.Services;
using FluentAssertions;
using MovieDiscussionTests.Fakers;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class GenerateAuthTokenTest : BaseTest
    {
        private readonly Faker<UserClaimsDto> _userClaimsFaker;

        public GenerateAuthTokenTest() : base()
        {
            _userClaimsFaker = UserRelatedFaker.GetUserClaimsDtoFaker();
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var userClaims = _userClaimsFaker.Generate();
            var candidates = new List<Token> { _tokenFaker.Generate() };
            var accessToken = "accessToken";
            var refreshToken = "refreshToken";

            _unitOfWork.Setup(unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken))
                .ReturnsAsync(candidates);
            _jwtProvider.Setup(jwt => jwt.GenerateToken(userClaims, TokenType.Access, Guid.Empty))
                .Returns((accessToken, It.IsAny<DateTime>()));
            _jwtProvider.Setup(jwt => jwt.GenerateToken(userClaims, TokenType.Refresh, It.IsAny<Guid>()))
                .Returns((refreshToken, It.IsAny<DateTime>()));


            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken);

            //Assert
            result.Should().Be((accessToken, refreshToken), "Auth tokens must be returned");

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.Delete(candidates[0], cancellationToken),
                Times.Once,
                "Delete must be called"
            );

            _jwtProvider.Verify(
                jwt => jwt.GenerateToken(userClaims, TokenType.Access, Guid.Empty),
                Times.Once,
                "Generate access token must be called"
            );

            _jwtProvider.Verify(
                jwt => jwt.GenerateToken(userClaims, TokenType.Refresh, It.IsAny<Guid>()),
                Times.Once,
                "Generate refresh token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.AddAsync(It.IsAny<Token>(), cancellationToken),
                Times.Once,
                "Add token must be called"
            );
        }

        [Fact]
        public async Task ShouldSucceed_WhenNoTokenFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var userClaims = _userClaimsFaker.Generate();
            var candidates = new List<Token> { };
            var accessToken = "accessToken";
            var refreshToken = "refreshToken";

            _unitOfWork.Setup(unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken))
                .ReturnsAsync(candidates);
            _jwtProvider.Setup(jwt => jwt.GenerateToken(userClaims, TokenType.Access, Guid.Empty))
                .Returns((accessToken, It.IsAny<DateTime>()));
            _jwtProvider.Setup(jwt => jwt.GenerateToken(userClaims, TokenType.Refresh, It.IsAny<Guid>()))
                .Returns((refreshToken, It.IsAny<DateTime>()));


            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken);

            //Assert
            result.Should().Be((accessToken, refreshToken), "Auth tokens must be returned");

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.Delete(It.IsAny<Token>(), cancellationToken),
                Times.Never,
                "Delete must be called"
            );

            _jwtProvider.Verify(
                jwt => jwt.GenerateToken(userClaims, TokenType.Access, Guid.Empty),
                Times.Once,
                "Generate access token must be called"
            );

            _jwtProvider.Verify(
                jwt => jwt.GenerateToken(userClaims, TokenType.Refresh, It.IsAny<Guid>()),
                Times.Once,
                "Generate refresh token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.AddAsync(It.IsAny<Token>(), cancellationToken),
                Times.Once,
                "Add token must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowInvalidOperation_WhenTooManyTokensFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var userClaims = _userClaimsFaker.Generate();
            var candidates = new List<Token> { 
                _tokenFaker.Generate(), 
                _tokenFaker.Generate() 
            };

            _unitOfWork.Setup(unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken))
                .ReturnsAsync(candidates);


            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowCancelledOperation_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var userClaims = _userClaimsFaker.Generate();

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");
        }
    }
}
