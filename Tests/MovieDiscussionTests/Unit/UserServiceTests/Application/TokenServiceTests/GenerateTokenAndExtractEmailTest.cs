using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Services;
using UserServiceDataAccess.DatabaseHandlers.Specifications.TokenSpecifications;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class GenerateTokenAndExtractEmailTest : BaseTest
    {
        public GenerateTokenAndExtractEmailTest() : base()
        {
        }

        [Theory]
        [InlineData("accessToken", TokenType.Access, true)]
        [InlineData("accessToken", TokenType.Refresh, true)]
        [InlineData("accessToken", TokenType.ResetPassword, true)]
        [InlineData("accessToken", TokenType.ConfirmEmail, true)]
        [InlineData("", TokenType.Access, true)]
        [InlineData("", TokenType.Refresh, true)]
        [InlineData("", TokenType.ResetPassword, true)]
        [InlineData("", TokenType.ConfirmEmail, true)]
        [InlineData("accessToken", TokenType.Access, false)]
        [InlineData("accessToken", TokenType.Refresh, false)]
        [InlineData("accessToken", TokenType.ResetPassword, false)]
        [InlineData("accessToken", TokenType.ConfirmEmail, false)]
        public async Task ShouldSucceed_WhenPassingValidData(string accessToken, TokenType tokenType, bool isAuth)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            var candidates = new List<Token> { _tokenFaker.Generate() };
            var returnTokenValue = "returnToken";

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(accessToken))
                .Returns(claimPrincipal);
            _unitOfWork.Setup(unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken))
                .ReturnsAsync(candidates);
            _jwtProvider.Setup(jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), tokenType, It.IsAny<Guid>()))
                .Returns((returnTokenValue, It.IsAny<DateTime>()));

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.GenerateTokenAndExtractEmailAsync(accessToken, tokenType, cancellationToken, isAuth);

            //Assert
            result.Should().NotBeNull("Result must be returned");
            result.Item1.Should().Be(returnTokenValue, "Token must be returned");
            result.Item2.Should().NotBeNull("Email must be returned");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(accessToken),
                Times.Once,
                "Get Principal from token must be called"
            );

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
                jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), tokenType, It.IsAny<Guid>()),
                Times.Once,
                "Generate refresh token must be called"
            );
        }

        [Theory]
        [InlineData("accessToken", TokenType.Access, true)]
        [InlineData("accessToken", TokenType.Refresh, true)]
        [InlineData("accessToken", TokenType.ResetPassword, true)]
        [InlineData("accessToken", TokenType.ConfirmEmail, true)]
        [InlineData("", TokenType.Access, true)]
        [InlineData("", TokenType.Refresh, true)]
        [InlineData("", TokenType.ResetPassword, true)]
        [InlineData("", TokenType.ConfirmEmail, true)]
        [InlineData("accessToken", TokenType.Access, false)]
        [InlineData("accessToken", TokenType.Refresh, false)]
        [InlineData("accessToken", TokenType.ResetPassword, false)]
        [InlineData("accessToken", TokenType.ConfirmEmail, false)]
        public async Task ShouldSucceed_WhenNoTokenFound(string accessToken, TokenType tokenType, bool isAuth)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            var candidates = new List<Token> { };
            var returnTokenValue = "returnToken";

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(accessToken))
                .Returns(claimPrincipal);
            _unitOfWork.Setup(unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken))
                .ReturnsAsync(candidates);
            _jwtProvider.Setup(jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), tokenType, It.IsAny<Guid>()))
                .Returns((returnTokenValue, It.IsAny<DateTime>()));

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.GenerateTokenAndExtractEmailAsync(accessToken, tokenType, cancellationToken, isAuth);

            //Assert
            result.Should().NotBeNull("Result must be returned");
            result.Item1.Should().Be(returnTokenValue, "Token must be returned");
            result.Item2.Should().NotBeNull("Email must be returned");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(accessToken),
                Times.Once,
                "Get Principal from token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.Delete(It.IsAny<Token>(), cancellationToken),
                Times.Never,
                "Delete must not be called"
            );

            _jwtProvider.Verify(
                jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), tokenType, It.IsAny<Guid>()),
                Times.Once,
                "Generate refresh token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.AddAsync(It.IsAny<Token>(), cancellationToken),
                Times.Once,
                "Add token must be called"
            );
        }

        [Theory]
        [InlineData("accessToken", TokenType.Access, true)]
        [InlineData("accessToken", TokenType.Refresh, true)]
        [InlineData("accessToken", TokenType.ResetPassword, true)]
        [InlineData("accessToken", TokenType.ConfirmEmail, true)]
        [InlineData("", TokenType.Access, true)]
        [InlineData("", TokenType.Refresh, true)]
        [InlineData("", TokenType.ResetPassword, true)]
        [InlineData("", TokenType.ConfirmEmail, true)]
        [InlineData("accessToken", TokenType.Access, false)]
        [InlineData("accessToken", TokenType.Refresh, false)]
        [InlineData("accessToken", TokenType.ResetPassword, false)]
        [InlineData("accessToken", TokenType.ConfirmEmail, false)]
        public async Task ShouldThrowTokenException_WhenPassingEmptyTokenNotForAuth(string accessToken, TokenType tokenType, bool isAuth)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            var candidates = new List<Token> { 
                _tokenFaker.Generate(), 
                _tokenFaker.Generate() 
            };

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(accessToken))
                .Returns(claimPrincipal);
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
                () => tokenService.GenerateTokenAndExtractEmailAsync(accessToken, tokenType, cancellationToken, isAuth)
            );

            //Assert
            exception.Should().NotBeNull();

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(accessToken),
                Times.Once,
                "Get Principal from token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );
        }

        [Theory]
        [InlineData(TokenType.Access)]
        [InlineData(TokenType.Refresh)]
        [InlineData(TokenType.ResetPassword)]
        [InlineData(TokenType.ConfirmEmail)]
        public async Task ShouldThrowInvalidOperationException_WhenTooManyCandidates(TokenType tokenType)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = "";
            var isAuth = false;

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<TokenException>(
                () => tokenService.GenerateTokenAndExtractEmailAsync(token, tokenType, cancellationToken, isAuth)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Token is not valid");
        }

        [Theory]
        [InlineData("accessToken", TokenType.Access, true)]
        [InlineData("accessToken", TokenType.Refresh, true)]
        [InlineData("accessToken", TokenType.ResetPassword, true)]
        [InlineData("accessToken", TokenType.ConfirmEmail, true)]
        [InlineData("", TokenType.Access, true)]
        [InlineData("", TokenType.Refresh, true)]
        [InlineData("", TokenType.ResetPassword, true)]
        [InlineData("", TokenType.ConfirmEmail, true)]
        [InlineData("accessToken", TokenType.Access, false)]
        [InlineData("accessToken", TokenType.Refresh, false)]
        [InlineData("accessToken", TokenType.ResetPassword, false)]
        [InlineData("accessToken", TokenType.ConfirmEmail, false)]
        public async Task ShouldThrowOperationCancelledException_WhenCancelling(string accessToken, TokenType tokenType, bool isAuth)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(accessToken))
                .Returns(claimPrincipal);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => tokenService.GenerateTokenAndExtractEmailAsync(accessToken, tokenType, cancellationToken, isAuth)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(accessToken),
                Times.Once,
                "Get Principal from token must be called"
            );
        }
    }
}
