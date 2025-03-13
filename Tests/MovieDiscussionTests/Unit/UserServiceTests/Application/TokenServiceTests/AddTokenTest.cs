using FluentAssertions;
using FluentValidation.Results;
using Moq;
using UserServiceApplication.Services;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class AddTokenTest : BaseTest
    {
        public AddTokenTest() : base()
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
            _unitOfWork.Setup(unit => unit.TokenRepository.AddAsync(token, cancellationToken))
                .ReturnsAsync(token.Id);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.AddTokenAsync(token, cancellationToken);

            //Assert
            result.Should().Be(token.Id, "Token id must be returned");

            _validator.Verify(
                validator => validator.Validate(token),
                Times.Once,
                "Validation must occur"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.AddAsync(token, cancellationToken),
                Times.Once,
                "Add token must be called"
            );
        }


        [Fact]
        public async Task ShouldThrowOperationCancelledExcpetion_WhenCancelling()
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
                () => tokenService.AddTokenAsync(token, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");
        }
    }
}
