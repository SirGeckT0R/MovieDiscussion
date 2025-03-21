using Bogus;
using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Dto;
using UserServiceApplication.Services;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class ResetPasswordTest : BaseTest
    {
        private readonly Faker<ResetPasswordRequest> _resetPasswordFaker;

        public ResetPasswordTest() : base()
        {
            _resetPasswordFaker = UserRelatedFaker.GetResetPasswordFaker();
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var resetPassword = _resetPasswordFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailTrackingAsync(resetPassword.Email.ToLower(), cancellationToken))
                .ReturnsAsync(user);
            _passwordHasher.Setup(hasher => hasher.Generate(resetPassword.NewPassword))
                .Returns(It.IsAny<string>());

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
            var result = await userService.ResetPasswordAsync(resetPassword, cancellationToken);

            //Assert
            result.Should().NotBeNull("Confirm token must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailTrackingAsync(resetPassword.Email.ToLower(), cancellationToken),
                Times.Once,
                "Get by email tracking must be called"
            );

            _tokenService.Verify(
                service => service.FindAndDeleteTokenAsync(It.IsAny<string>(), TokenType.ResetPassword, cancellationToken),
                Times.Once,
                "Find and delete token must be called"
            );

            _passwordHasher.Verify(
                hasher => hasher.Generate(resetPassword.NewPassword), 
                Times.Once, 
                "Password generate must be called"
            );

            _unitOfWork.Verify(
                unit => unit.SaveChangesAsync(cancellationToken),
                Times.Once,
                "Save Changes must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenNoUserFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var resetPassword = _resetPasswordFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailTrackingAsync(resetPassword.Email.ToLower(), cancellationToken))
                .ReturnsAsync((User?) null);

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
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => userService.ResetPasswordAsync(resetPassword, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailTrackingAsync(resetPassword.Email.ToLower(), cancellationToken),
                Times.Once,
                "Get by email tracking must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowCancelledException_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var resetPassword = _resetPasswordFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailTrackingAsync(resetPassword.Email.ToLower(), cancellationToken))
                .ReturnsAsync(user);

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
                () => userService.ResetPasswordAsync(resetPassword, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailTrackingAsync(resetPassword.Email.ToLower(), cancellationToken),
                Times.Once,
                "Get by email tracking must be called"
            );
        }
    }
}
