using Bogus;
using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Dto;
using UserServiceApplication.Services;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class GetByTokenTest : BaseTest
    {
        private readonly Faker<UserClaimsDto> _userClaimsDtoFaker;
        private readonly Faker<UserDto> _userDtoFaker;

        public GetByTokenTest() : base()
        {
            _userClaimsDtoFaker = UserRelatedFaker.GetUserClaimsDtoFaker();
            _userDtoFaker = UserRelatedFaker.GetUserDtoFaker();
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = "accessToken";
            var user = _userFaker.Generate();
            var userClaims = _userClaimsDtoFaker.Generate();
            var userDto = _userDtoFaker.Generate();

            _tokenService.Setup(service => service.ExtractClaims(token))
                .Returns((It.IsAny<Guid>(), userClaims));
            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(userClaims.Id, cancellationToken))
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
            var result = await userService.GetUserByTokenAsync(token, cancellationToken);

            //Assert
            result.Should().Be(userDto, "User dto must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(userClaims.Id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );

            _tokenService.Verify(
                service => service.ExtractClaims(token),
                Times.Once,
                "Extract claims must be called"
            );

            _mapper.Verify(
                mapper => mapper.Map<UserDto>(user),
                Times.Once,
                "Mapping to user dto must occur"
            );
        }

        [Fact]
        public async Task ShouldThrowTokenException_WhenTokenIsEmpty()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = "";

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
            var exception = await Assert.ThrowsAsync<TokenException>(
                () => userService.GetUserByTokenAsync(token, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Token is not valid");
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenUserNotFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var token = "accessToken";
            var userClaims = _userClaimsDtoFaker.Generate();

            _tokenService.Setup(service => service.ExtractClaims(token))
                .Returns((It.IsAny<Guid>(), userClaims));
            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(userClaims.Id, cancellationToken))
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
                () => userService.GetUserByTokenAsync(token, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");

            _tokenService.Verify(
                service => service.ExtractClaims(token),
                Times.Once,
                "Extract claims must be called"
            );

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(userClaims.Id, cancellationToken),
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

            var token = "accessToken";
            var user = _userFaker.Generate();
            var userClaims = _userClaimsDtoFaker.Generate();

            _tokenService.Setup(service => service.ExtractClaims(token))
                .Returns((It.IsAny<Guid>(), userClaims));
            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(userClaims.Id, cancellationToken))
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
                () => userService.GetUserByTokenAsync(token, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _tokenService.Verify(
                service => service.ExtractClaims(token),
                Times.Once,
                "Extract claims must be called"
            );

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(userClaims.Id, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );
        }
    }
}
