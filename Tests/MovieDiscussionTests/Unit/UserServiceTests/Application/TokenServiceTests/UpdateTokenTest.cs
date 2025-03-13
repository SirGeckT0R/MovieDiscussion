using FluentAssertions;
using FluentValidation.Results;
using Moq;
using UserServiceApplication.Services;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class UpdateTokenTest : BaseTest
    {
        public UpdateTokenTest() : base()
        {
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = _tokenFaker.Generate();
            var validationResult = new ValidationResult() { Errors = new List<ValidationFailure>() };

            _validator.Setup(validator => validator.Validate(token))
                .Returns(validationResult);
            _unitOfWork.Setup(unit => unit.TokenRepository.GetByIdAsync(token.Id, cancellationToken))
                .ReturnsAsync(token);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            await tokenService.UpdateTokenAsync(token, cancellationToken);

            //Assert
            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetByIdAsync(token.Id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );

            _validator.Verify(
                validator => validator.Validate(token),
                Times.Once,
                "Validation must occur"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.Update(token, cancellationToken),
                Times.Once,
                "Update must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenNoUserFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = _tokenFaker.Generate();

            _unitOfWork.Setup(unit => unit.TokenRepository.GetByIdAsync(token.Id, cancellationToken))
                .ReturnsAsync((Token?) null);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => tokenService.UpdateTokenAsync(token, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("No token was found");

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetByIdAsync(token.Id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowOperationCancelledException_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var token = _tokenFaker.Generate();

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => tokenService.UpdateTokenAsync(token, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");
        }
    }
}
