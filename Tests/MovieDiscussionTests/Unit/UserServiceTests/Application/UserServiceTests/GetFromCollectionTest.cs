using Bogus;
using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Dto;
using UserServiceApplication.Services;
using UserServiceDataAccess.DatabaseHandlers.Specifications.UserSpecifications;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class GetFromCollectionTest : BaseTest
    {
        private readonly Faker<UserDto> _userDtoFaker;

        public GetFromCollectionTest() : base()
        {
            _userDtoFaker = UserRelatedFaker.GetUserDtoFaker();
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var ids = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };
            var users = new List<User>
            {
                _userFaker.Generate(),
                _userFaker.Generate(),
                _userFaker.Generate(),
            };
            var usersDto = new List<UserDto>
            {
                _userDtoFaker.Generate(),
                _userDtoFaker.Generate(),
                _userDtoFaker.Generate(),
            };

            _unitOfWork.Setup(unit => unit.UserRepository.GetWithSpecificationAsync(It.IsAny<UsersFromCollectionSpecification>(), cancellationToken))
                .ReturnsAsync(users);
            _mapper.Setup(mapper => mapper.Map<ICollection<UserDto>>(users))
                .Returns(usersDto);

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
            var result = await userService.GetFromCollectionAsync(ids, cancellationToken);

            //Assert
            result.Should().BeEquivalentTo(usersDto, "User dtos must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetWithSpecificationAsync(It.IsAny<UsersFromCollectionSpecification>(), cancellationToken),
                Times.Once,
                "Get with specification must be called"
            );

            _mapper.Verify(
                mapper => mapper.Map<ICollection<UserDto>>(users),
                Times.Once,
                "Mapping to user dtos must occur"
            );
        }

        [Fact]
        public async Task ShouldThrowCancelledException_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var ids = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };
            var users = new List<User>
            {
                _userFaker.Generate(),
                _userFaker.Generate(),
                _userFaker.Generate(),
            };

            _unitOfWork.Setup(unit => unit.UserRepository.GetWithSpecificationAsync(It.IsAny<UsersFromCollectionSpecification>(), cancellationToken))
                    .ReturnsAsync(users);

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
                () => userService.GetFromCollectionAsync(ids, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetWithSpecificationAsync(It.IsAny<UsersFromCollectionSpecification>(), cancellationToken),
                Times.Once,
                "Get with specification must be called"
            );
        }
    }
}
