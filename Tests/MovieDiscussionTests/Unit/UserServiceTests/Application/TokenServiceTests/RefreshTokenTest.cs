using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Services;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class RefreshTokenTest : BaseTest
    {
        public RefreshTokenTest() :base()
        {
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = "inputToken";
            var returnTokenValue = "returnToken";
            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();
            var candidate = _tokenFaker.Generate();

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(token))
                .Returns(claimPrincipal);
            _unitOfWork.Setup(unit => unit.TokenRepository.GetByIdAsync(It.IsAny<Guid>(), cancellationToken))
                .ReturnsAsync(candidate);
            _jwtProvider.Setup(jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), TokenType.Access, It.IsAny<Guid>()))
                .Returns((returnTokenValue, It.IsAny<DateTime>()));

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.RefreshTokenAsync(token, cancellationToken);

            //Assert
            result.Should().Be(returnTokenValue, "Access token must be returned");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(token),
                Times.Once,
                "Get Principal from token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetByIdAsync(It.IsAny<Guid>(), cancellationToken),
                Times.Once,
                "Get by id must be called"
            );

            _jwtProvider.Verify(
                jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), TokenType.Access, It.IsAny<Guid>()),
                Times.Once,
                "Generate token must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowTokenException_WhenTokenIsEmpty()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = "";

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<TokenException>(
                () => tokenService.RefreshTokenAsync(token, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Token is empty");
        }

        [Fact]
        public async Task ShouldThrowOperationCancelledException_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var token = "inputToken";
            var claimPrincipal = UserRelatedFaker.GetUserPrincipal();

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(token))
                .Returns(claimPrincipal);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => tokenService.RefreshTokenAsync(token, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(token),
                Times.Once,
                "Get Principal from token must be called"
            );
        }
    }
}
