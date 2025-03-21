using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Services;
using UserServiceDataAccess.DatabaseHandlers.Specifications.TokenSpecifications;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class FindAndDeleteTokenTest : BaseTest
    {
        public FindAndDeleteTokenTest() : base()
        {
        }

        [Theory]
        [InlineData(TokenType.Access)]
        [InlineData(TokenType.ResetPassword)]
        [InlineData(TokenType.ConfirmEmail)]
        [InlineData(TokenType.Refresh)]
        public async Task ShouldSucceed_WhenPassingValidData(TokenType tokenType)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var confirmToken = "confirmToken";
            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            var candidates = new List<Token> { _tokenFaker.Generate() };

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(confirmToken))
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
            await tokenService.FindAndDeleteTokenAsync(confirmToken, tokenType, cancellationToken);

            //Assert
            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(confirmToken),
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
        }

        [Theory]
        [InlineData(TokenType.Access)]
        [InlineData(TokenType.ResetPassword)]
        [InlineData(TokenType.ConfirmEmail)]
        [InlineData(TokenType.Refresh)]
        public async Task ShouldThrowTokenException_WhenNotTokenFound(TokenType tokenType)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var confirmToken = "";

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<TokenException>(
                () => tokenService.FindAndDeleteTokenAsync(confirmToken, tokenType, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Token is not valid");
        }

        [Theory]
        [InlineData(TokenType.Access)]
        [InlineData(TokenType.ResetPassword)]
        [InlineData(TokenType.ConfirmEmail)]
        [InlineData(TokenType.Refresh)]
        public async Task ShouldThrowNotFoundException_WhenNotTokenFound(TokenType tokenType)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var confirmToken = "confirmToken";
            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            var candidates = new List<Token> { };

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(confirmToken))
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
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => tokenService.FindAndDeleteTokenAsync(confirmToken, tokenType, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Token not found");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(confirmToken),
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
        [InlineData(TokenType.ResetPassword)]
        [InlineData(TokenType.ConfirmEmail)]
        [InlineData(TokenType.Refresh)]
        public async Task ShouldThrowInvalidOperationException_WhenTooManyCandidates(TokenType tokenType)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var confirmToken = "confirmToken";
            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            var candidates = new List<Token> { _tokenFaker.Generate(), _tokenFaker.Generate() };

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(confirmToken))
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
                () => tokenService.FindAndDeleteTokenAsync(confirmToken, tokenType, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(confirmToken),
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
        [InlineData(TokenType.ResetPassword)]
        [InlineData(TokenType.ConfirmEmail)]
        [InlineData(TokenType.Refresh)]
        public async Task ShouldThrowOperationCancelled_WhenCancelling(TokenType tokenType)
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var confirmToken = "confirmToken";
            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            var candidates = new List<Token> { _tokenFaker.Generate() };

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(confirmToken))
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
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => tokenService.FindAndDeleteTokenAsync(confirmToken, tokenType, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(confirmToken),
                Times.Once,
                "Get Principal from token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );
        }
    }
}
