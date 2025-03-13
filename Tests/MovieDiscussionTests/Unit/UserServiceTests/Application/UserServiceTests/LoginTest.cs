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
    public class LoginTest : BaseTest
    {
        private readonly Faker<UserClaimsDto> _userClaimsFaker;
        private readonly Faker<LoginRequest> _loginRequestFaker;

        public LoginTest() : base()
        {
            _userClaimsFaker = UserRelatedFaker.GetUserClaimsDtoFaker();
            _loginRequestFaker = UserRelatedFaker.GetLoginRequestFaker();
        }

        [Fact]
        public async Task ShouldReturnTokens_WhenPassingValidCredentials()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var loginCommand = _loginRequestFaker.Generate();
            var user = _userFaker.Generate();
            var userClaims = _userClaimsFaker.Generate();
            (string accessToken, string refreshToken) tokens = ("accessToken", "refreshToken");

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(loginCommand.Email.ToLower(), cancellationToken))
                .ReturnsAsync(user);
            _passwordHasher.Setup(hasher => hasher.Verify(loginCommand.Password, user.Password))
                .Returns(true);
            _mapper.Setup(mapper => mapper.Map<UserClaimsDto>(user))
                .Returns(userClaims);
            _tokenService.Setup(service => service.GenerateAuthTokensAsync(userClaims, cancellationToken))
                .ReturnsAsync(tokens);

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
            var result = await userService.LoginAsync(loginCommand, cancellationToken);

            //Assert
            result.Should().Be(tokens, "Tokens must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(loginCommand.Email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );

            _unitOfWork.Verify(
                unit => unit.SaveChangesAsync(cancellationToken),
                Times.Once,
                "Save Changes must be called"
            );

            _passwordHasher.Verify(
                hasher => hasher.Verify(loginCommand.Password, user.Password),
                Times.Once,
                "Verify password must be called"
            );

            _mapper.Verify(
                mapper => mapper.Map<UserClaimsDto>(user),
                Times.Once,
                "Mapping to user claims must occur"
            );

            _tokenService.Verify(
                service => service.GenerateAuthTokensAsync(userClaims, cancellationToken),
                Times.Once,
                "Generate auth tokens must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenEmailIncorrect()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var loginCommand = _loginRequestFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(loginCommand.Email.ToLower(), cancellationToken))
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
                () => userService.LoginAsync(loginCommand, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Email or password is incorrect"); 
            
            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(loginCommand.Email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenPasswordIsIncorrect()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var loginCommand = _loginRequestFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(loginCommand.Email.ToLower(), cancellationToken))
                .ReturnsAsync(user);
            _passwordHasher.Setup(hasher => hasher.Verify(loginCommand.Password, user.Password))
                .Returns(false);

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
                () => userService.LoginAsync(loginCommand, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Email or password is incorrect");
        }

        [Fact]
        public async Task ShouldThrowOperationCancelled_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var loginCommand = _loginRequestFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(loginCommand.Email.ToLower(), cancellationToken))
                .ReturnsAsync(user);
            _passwordHasher.Setup(hasher => hasher.Verify(loginCommand.Password, user.Password))
                .Returns(true);

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
                () => userService.LoginAsync(loginCommand, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");
        }
    }
}
