using FluentAssertions;
using Moq;
using UserServiceApplication.Services;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class GetTokenTest : BaseTest
    {
        public GetTokenTest() : base()
        {
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var tokenId = Guid.NewGuid();
            var token = _tokenFaker.Generate();

            _unitOfWork.Setup(unit => unit.TokenRepository.GetByIdAsync(tokenId, cancellationToken))
                .ReturnsAsync(token);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.GetTokenAsync(tokenId, cancellationToken);

            //Assert
            result.Should().Be(token, "Token must be returned");

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetByIdAsync(tokenId, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowOperationCancelledExcpetion_WhenCancellinga()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var tokenId = Guid.NewGuid();

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => tokenService.GetTokenAsync(tokenId, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");
        }
    }
}
