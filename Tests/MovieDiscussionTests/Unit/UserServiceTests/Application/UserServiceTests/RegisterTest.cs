using Bogus;
using FluentAssertions;
using FluentValidation.Results;
using Grpc.Core;
using Moq;
using MovieDiscussionTests.Fakers;
using MovieGrpcClient;
using UserServiceApplication.Dto;
using UserServiceApplication.Services;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class RegisterTest : BaseTest
    {
        private readonly Faker<UserClaimsDto> _userClaimsFaker;
        private readonly Faker<RegisterRequest> _registerRequestFaker;

        public RegisterTest() : base()
        {
            _userClaimsFaker = UserRelatedFaker.GetUserClaimsDtoFaker();
            _registerRequestFaker = UserRelatedFaker.GetRegisterRequestFaker();
        }

        [Fact]
        public async Task ShouldReturnTokens_WhenPassingValidCredentials()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var registerCommand = _registerRequestFaker.Generate();
            var user = _userFaker.Generate();
            var grpcReply = UserRelatedFaker.GetGrpcCreateReply();
            var validationResult = new FluentValidation.Results.ValidationResult() { Errors = new List<ValidationFailure>() };
            var userClaims = _userClaimsFaker.Generate();
            (string accessToken, string refreshToken) tokens = ("accessToken", "refreshToken");


            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(registerCommand.Email.ToLower(), cancellationToken))
                .ReturnsAsync((User?) null);
            _passwordHasher.Setup(hasher => hasher.Generate(user.Password))
                .Returns(user.Password);
            _mapper.Setup(mapper => mapper.Map<User>(registerCommand))
                .Returns(user);
            _validator.Setup(validator => validator.Validate(user))
                .Returns(validationResult);
            _unitOfWork.Setup(unit => unit.UserRepository.AddAsync(user, cancellationToken))
                .ReturnsAsync(user.Id);
            _mapper.Setup(mapper => mapper.Map<UserClaimsDto>(user))
                .Returns(userClaims);
            _client.Setup(client => client.CreateProfileAndWatchlistAsync(It.IsAny<CreateProfileAndWatchlistRequest>(), It.IsAny<CallOptions>()))
                .Returns(grpcReply);
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
            var result = await userService.RegisterAsync(registerCommand, cancellationToken);

            //Assert
            result.Should().Be(tokens, "Tokens must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(registerCommand.Email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );

            _unitOfWork.Verify(
                unit => unit.UserRepository.AddAsync(user, cancellationToken),
                Times.Once,
                "AddAsync must be called"
            );

            _unitOfWork.Verify(
                unit => unit.SaveChangesAsync(cancellationToken),
                Times.Once,
                "Save Changes must be called"
            );

            _passwordHasher.Verify(
                hasher => hasher.Generate(user.Password),
                Times.Once,
                "Generate password must be called"
            );

            _validator.Verify(
                validator => validator.Validate(user),
                Times.Once,
                "Validation must occur"
            );

            _client.Verify(
                client => client.CreateProfileAndWatchlistAsync(It.IsAny<CreateProfileAndWatchlistRequest>(), It.IsAny<CallOptions>()),
                Times.Once,
                "Call to grpc service must occur"
            );

            _mapper.Verify(
                mapper => mapper.Map<User>(registerCommand),
                Times.Once,
                "Mapping to user must occur"
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
        public async Task ShouldThrowConflictException_WhenUserAlreadyExists()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var registerCommand = _registerRequestFaker.Generate();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(registerCommand.Email.ToLower(), cancellationToken))
                .ReturnsAsync(user);
            _passwordHasher.Setup(hasher => hasher.Generate(registerCommand.Password))
                .Returns(registerCommand.Password);

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
            var exception = await Assert.ThrowsAsync<ConflictException>(
                () => userService.RegisterAsync(registerCommand, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User already exists");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(registerCommand.Email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowOperationCancelled_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var registerCommand = _registerRequestFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(registerCommand.Email.ToLower(), cancellationToken))
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
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => userService.RegisterAsync(registerCommand, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(registerCommand.Email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );
        }
    }
}
