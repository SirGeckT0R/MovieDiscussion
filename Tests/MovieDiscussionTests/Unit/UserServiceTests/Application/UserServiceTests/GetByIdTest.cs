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
    public class GetByIdTest : BaseTest
    {
        private readonly Faker<UserDto> _userDtoFaker;

        public GetByIdTest() : base()
        {
            _userDtoFaker = UserRelatedFaker.GetUserDtoFaker();
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var id = Guid.NewGuid();
            var user = _userFaker.Generate();
            var userDto = _userDtoFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(id, cancellationToken))
                .ReturnsAsync(user);
            _mapper.Setup(mapper => mapper.Map<UserDto>(user))
                .Returns(userDto);

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
            var result = await userService.GetUserByIdAsync(id, cancellationToken);

            //Assert
            result.Should().Be(userDto, "User dto must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );

            _mapper.Verify(
                mapper => mapper.Map<UserDto>(user),
                Times.Once,
                "Mapping to user dto must occur"
            );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenNoUserFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var id = Guid.NewGuid();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(id, cancellationToken))
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
                () => userService.GetUserByIdAsync(id, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(id, cancellationToken),
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

            var id = Guid.NewGuid();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(id, cancellationToken))
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
                () => userService.GetUserByIdAsync(id, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );
        }
    }
}
