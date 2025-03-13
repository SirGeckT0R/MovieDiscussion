using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Services;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class ForgotPasswordTest : BaseTest
    {
        public ForgotPasswordTest() : base()
        {
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var email = UserRelatedFaker.GetEmail();
            var callbackUrl = "callbackUrl";
            var resetToken = "resetToken";

            _tokenService.Setup(service => service.GenerateResetTokenAsync(email.ToLower(), cancellationToken))
                .ReturnsAsync(resetToken);

            var userService = new UserService(
                                                _unitOfWork.Object,
                                                _tokenService.Object,
                                                _mapper.Object,
                                                _passwordHasher.Object,
                                                _validator.Object,
                                                _emailService.Object,
                                                _logger.Object,
                                                _client.Object,
                                                _configuration.Object,
                                                _backgroundClient.Object
                                              );

            //Act
            var result = await userService.ForgotPasswordAsync(email, callbackUrl, cancellationToken);

            //Assert
            result.Should().NotBeNull("Reset token must be returned");

            _tokenService.Verify(
                service => service.GenerateResetTokenAsync(email.ToLower(), cancellationToken),
                Times.Once,
                "Generate reset token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.SaveChangesAsync(cancellationToken),
                Times.Once,
                "Save Changes must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowCancelledException_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var email = UserRelatedFaker.GetEmail();
            var callbackUrl = "callbackUrl";
            var resetToken = "resetToken";

            _tokenService.Setup(service => service.GenerateResetTokenAsync(email.ToLower(), cancellationToken))
                .ReturnsAsync(resetToken);

            var userService = new UserService(
                                                _unitOfWork.Object,
                                                _tokenService.Object,
                                                _mapper.Object,
                                                _passwordHasher.Object,
                                                _validator.Object,
                                                _emailService.Object,
                                                _logger.Object,
                                                _client.Object,
                                                _configuration.Object,
                                                _backgroundClient.Object
                                              );

            //Act
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => userService.ForgotPasswordAsync(email, callbackUrl, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _tokenService.Verify(
                service => service.GenerateResetTokenAsync(email.ToLower(), cancellationToken),
                Times.Once,
                "Generate reset token must be called"
            );
        }
    }
}
