using FluentAssertions;
using Moq;
using UserServiceApplication.Services;
using UserServiceDataAccess.Enums;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class ChangePasswordTest : BaseTest
    {
        public ChangePasswordTest() : base()
        {
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = "accessToken";
            var callbackUrl = "callbackUrl";

            var resetToken = "resetToken";

            _tokenService.Setup(service => service.GenerateTokenAndExtractEmailAsync(token, TokenType.ResetPassword, cancellationToken, false))
                .ReturnsAsync((resetToken, It.IsAny<string>()));

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
            var result = await userService.ChangePasswordAsync(token, callbackUrl, cancellationToken);

            //Assert
            result.Should().NotBeNull("Reset token must be returned");

            _tokenService.Verify(
                service => service.GenerateTokenAndExtractEmailAsync(token, TokenType.ResetPassword, cancellationToken, false),
                Times.Once,
                "Generate token and extract email must be called"
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

            var token = "accessToken";
            var callbackUrl = "callbackUrl";

            var resetToken = "resetToken";

            _tokenService.Setup(service => service.GenerateTokenAndExtractEmailAsync(token, TokenType.ResetPassword, cancellationToken, false))
                .ReturnsAsync((resetToken, It.IsAny<string>()));

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
                () => userService.ChangePasswordAsync(token, callbackUrl, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _tokenService.Verify(
                service => service.GenerateTokenAndExtractEmailAsync(token, TokenType.ResetPassword, cancellationToken, false),
                Times.Once,
                "Generate token and extract email must be called"
                );
        }
    }
}
