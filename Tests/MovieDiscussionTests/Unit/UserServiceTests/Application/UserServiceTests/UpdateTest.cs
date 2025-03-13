using Bogus;
using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Dto;
using UserServiceApplication.Services;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class UpdateTest : BaseTest
    {
        private readonly Faker<UpdateUserRequest> _updateUserRequestFaker;

        public UpdateTest() : base()
        {
            _updateUserRequestFaker = UserRelatedFaker.GetUpdateUserRequestFaker();
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var updateCommand = _updateUserRequestFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(updateCommand.Id, cancellationToken))
                .ReturnsAsync(user);
            _mapper.Setup(mapper => mapper.Map(updateCommand, user))
                .Returns(user);

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
            await userService.UpdateUserAsync(updateCommand, cancellationToken);

            //Assert
            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(updateCommand.Id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );

            _unitOfWork.Verify(
                unit => unit.UserRepository.Update(user, cancellationToken),
                Times.Once,
                "Update must be called"
            );

            _unitOfWork.Verify(
                unit => unit.SaveChangesAsync(cancellationToken),
                Times.Once,
                "Save Changes must be called"
            );

            _mapper.Verify(
                mapper => mapper.Map(updateCommand, user),
                Times.Once,
                "Mapping to user must occur"
            );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenNoUserFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var updateCommand = _updateUserRequestFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(updateCommand.Id, cancellationToken))
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
                () => userService.UpdateUserAsync(updateCommand, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(updateCommand.Id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowCancelledException_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var updateCommand = _updateUserRequestFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(updateCommand.Id, cancellationToken))
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
                () => userService.UpdateUserAsync(updateCommand, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(updateCommand.Id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );
        }
    }
}
