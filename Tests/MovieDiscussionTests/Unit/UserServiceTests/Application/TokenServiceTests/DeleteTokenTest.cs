using FluentAssertions;
using Moq;
using UserServiceApplication.Services;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class DeleteTokenTest : BaseTest
    {
        public DeleteTokenTest() : base()
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
            await tokenService.DeleteTokenAsync(tokenId, cancellationToken);

            //Assert
            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetByIdAsync(tokenId, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.Delete(token, cancellationToken),
                Times.Once,
                "Delete must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenNoUserFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var tokenId = Guid.NewGuid();

            _unitOfWork.Setup(unit => unit.TokenRepository.GetByIdAsync(tokenId, cancellationToken))
                .ReturnsAsync((Token?) null);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => tokenService.DeleteTokenAsync(tokenId, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("No token was found");

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
                () => tokenService.DeleteTokenAsync(tokenId, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");
        }
    }
}
