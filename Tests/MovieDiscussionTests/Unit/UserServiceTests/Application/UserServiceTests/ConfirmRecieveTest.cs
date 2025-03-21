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
    public class ConfirmRecieveTest : BaseTest
    {
        private readonly Faker<ConfirmEmailRequest> _confirmEmailFaker;

        public ConfirmRecieveTest() : base()
        {
            _confirmEmailFaker = UserRelatedFaker.GetConfirmEmailFaker();
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var confirmRequest = _confirmEmailFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailTrackingAsync(confirmRequest.Email, cancellationToken))
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
            var result = await userService.ConfirmEmailRecieveAsync(confirmRequest, cancellationToken);

            //Assert
            result.Should().NotBeNull("Confirm token must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailTrackingAsync(confirmRequest.Email, cancellationToken),
                Times.Once,
                "Get by email tracking must be called"
            );

            _tokenService.Verify(
                service => service.FindAndDeleteTokenAsync(It.IsAny<string>(), TokenType.ConfirmEmail, cancellationToken),
                Times.Once,
                "Find and delete token must be called"
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

            var confirmRequest = _confirmEmailFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailTrackingAsync(confirmRequest.Email, cancellationToken))
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
                () => userService.ConfirmEmailRecieveAsync(confirmRequest, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailTrackingAsync(confirmRequest.Email, cancellationToken),
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

            var confirmRequest = _confirmEmailFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailTrackingAsync(confirmRequest.Email, cancellationToken))
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
                () => userService.ConfirmEmailRecieveAsync(confirmRequest, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailTrackingAsync(confirmRequest.Email, cancellationToken),
                Times.Once,
                "Get by email tracking must be called"
            );
        }
    }
}
