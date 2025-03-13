using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UserServiceApplication.Dto;
using UserServiceApplication.Interfaces.Services;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Integration.UserService
{
    public class UserIntegrationTest : IntegrationTest
    {
        private readonly TestWebClientFactory _clientFactory;
        private readonly IServiceScope _scope;
        private readonly IUserService userService;

        public UserIntegrationTest(TestWebClientFactory factory) : base()
        {
            _clientFactory = factory;
            _scope = _clientFactory.Services.CreateScope();
            userService = _scope.ServiceProvider.GetRequiredService<IUserService>();
        }

        [Fact]
        public async Task Login_ShouldReturnTokens_WhenCredentialsValid()
        {
            //Arrange
            var request = new LoginRequest() { 
                Email = "example@example.com", 
                Password = "cool" 
            };

            //Act
            var result = await userService.LoginAsync(request, default);

            //Assert
            result.Should().BeOfType<(string, string)>();
            result.Item1.Should().NotBeNull();
            result.Item2.Should().NotBeNull();
        }

        [Fact]
        public async Task Login_ShouldThrowNotFoundException_WhenEmailNotValid()
        {
            //Arrange
            var request = new LoginRequest() { 
                Email = "notexample@example.com", 
                Password = "cool" 
            };

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => userService.LoginAsync(request, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Email or password is incorrect");
        }

        [Fact]
        public async Task Login_ShouldThrowNotFoundException_WhenPasswordNotValid()
        {
            //Arrange
            var request = new LoginRequest() { 
                Email = "example@example.com", 
                Password = "cool1" 
            };

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => userService.LoginAsync(request, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Email or password is incorrect");
        }

        [Fact]
        public async Task Register_ShouldReturnTokens_WhenCredentialsValid()
        {
            //Arrange
            var request = new RegisterRequest() { 
                Username = "A", 
                Email = "new@example.com", 
                Password = "cool" 
            };

            //Act
            var result = await userService.RegisterAsync(request, default);

            //Assert
            result.Should().BeOfType<(string, string)>();
            result.Item1.Should().NotBeNull();
            result.Item2.Should().NotBeNull();
        }

        [Fact]
        public async Task Register_ShouldConflictException_WhenUserAlredyExists()
        {
            //Arrange
            var request = new RegisterRequest() { 
                Username = "A", 
                Email = "example@example.com", 
                Password = "cool" 
            };

            //Act
            var exception = await Assert.ThrowsAsync<ConflictException>(
                () => userService.RegisterAsync(request, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User already exists");
        }

        [Theory]
        [InlineData("A", "testnew@example.com", "")]
        [InlineData("", "1new@example.com", "cool")]
        [InlineData("AB", "", "cool")]
        [InlineData("", "", "cool")]
        [InlineData("", "", "")]
        [InlineData("", "2new@example.com", "")]
        [InlineData("AC", "", "")]
        public async Task Register_ShouldThrowValidationException_WhenDataIsNotCorrect(string username, string email, string password)
        {
            //Arrange
            var request = new RegisterRequest() { 
                Username = username, 
                Email = email, 
                Password = password 
            };

            //Act
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => userService.RegisterAsync(request, default)
            );

            //Assert
            exception.Should().NotBeNull();
        }

        [Fact]
        public async Task Change_ShouldReturnResetToken_WhenDataIsValid()
        {
            //Arrange
            var callbackUrl = "callbackUrl";

            var jwtProvider = _scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            var userClaims = new UserClaimsDto(Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29"), "example@example.com", Role.User);
            var (accessToken, _) = jwtProvider.GenerateToken(userClaims, TokenType.Access);

            //Act
            var result = await userService.ChangePasswordAsync(accessToken, callbackUrl, default);

            //Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Forgot_ShouldReturnResetToken_WhenDataIsValid()
        {
            //Arrange
            var email = "example@example.com";
            var callbackUrl = "callbackUrl";

            //Act
            var result = await userService.ForgotPasswordAsync(email, callbackUrl, default);

            //Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Forgot_ShouldThrowBadRequestException_WhenEmailIsEmpty()
        {
            //Arrange
            var email = "";
            var callbackUrl = "callbackUrl";

            //Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => userService.ForgotPasswordAsync(email, callbackUrl, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Email is not valid");
        }

        [Fact]
        public async Task Reset_ShouldResetPassword_WhenChange()
        {
            //Arrange
            var email = "example@example.com";
            var callbackUrl = "callbackUrl";
            var newPassword = "new1";

            var unitOfWork = _scope.ServiceProvider.GetRequiredService<IUserUnitOfWork>();
            var passwordHasher = _scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            var jwtProvider = _scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            var userClaims = new UserClaimsDto(Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29"), "example@example.com", Role.User);
            var (accessToken, _) = jwtProvider.GenerateToken(userClaims, TokenType.Access);

            var token = await userService.ChangePasswordAsync(accessToken, callbackUrl, default);

            var resetPassword = new ResetPasswordRequest { 
                Email = email, 
                NewPassword = newPassword, 
                Token = token 
            };

            //Act
            var result = await userService.ResetPasswordAsync(resetPassword, default);

            //Assert
            var user = await unitOfWork.UserRepository.GetByEmailAsync(email, default);

            result.Should().NotBeEmpty();
            user.Should().NotBeNull();
            passwordHasher.Verify(newPassword, user.Password).Should().BeTrue();
        }

        [Fact]
        public async Task Reset_ShouldResetPassword_WhenForgot()
        {
            //Arrange
            var email = "example@example.com";
            var callbackUrl = "callbackUrl";
            var newPassword = "new";

            var unitOfWork = _scope.ServiceProvider.GetRequiredService<IUserUnitOfWork>();
            var passwordHasher = _scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            var token = await userService.ForgotPasswordAsync(email, callbackUrl, default);

            var resetPassword = new ResetPasswordRequest { 
                Email = email, 
                NewPassword = newPassword, 
                Token = token 
            };

            //Act
            var result = await userService.ResetPasswordAsync(resetPassword, default);

            //Assert
            var user = await unitOfWork.UserRepository.GetByEmailAsync(email, default);

            result.Should().NotBeEmpty();
            user.Should().NotBeNull();
            passwordHasher.Verify(newPassword, user.Password).Should().BeTrue();
        }

        [Fact]
        public async Task Reset_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            //Arrange
            var email = "notcorrect@example.com";
            var newPassword = "new";

            var resetPassword = new ResetPasswordRequest { Email = email, NewPassword = newPassword, Token = "token" };

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => userService.ResetPasswordAsync(resetPassword, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task Delete_ShouldSucceed_WhenDataIsValid()
        {
            //Arrange
            var id = Guid.Parse("2de57bf5-c74e-4d2f-b803-0ea8e6dc63ce");

            var unitOfWork = _scope.ServiceProvider.GetRequiredService<IUserUnitOfWork>();

            //Act
            await userService.DeleteAsync(id, default);

            //Assert
            var user = await unitOfWork.UserRepository.GetByIdAsync(id, default);
            user.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => userService.DeleteAsync(id, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task Update_ShouldSucceed_WhenDataIsValid()
        {
            //Arrange
            var id = Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29");
            var username = "newUsername";
            var updateUserRequest = new UpdateUserRequest { Id = id, UserName = username };

            var unitOfWork = _scope.ServiceProvider.GetRequiredService<IUserUnitOfWork>();

            //Act
            await userService.UpdateUserAsync(updateUserRequest, default);

            //Assert
            var user = await unitOfWork.UserRepository.GetByIdAsync(id, default);
            user.Should().NotBeNull();
            user.Username.Should().Be(username);
        }

        [Fact]
        public async Task Update_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            //Arrange
            var id = Guid.NewGuid();
            var username = "newUsername";
            var updateUserRequest = new UpdateUserRequest { Id = id, UserName = username };

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => userService.UpdateUserAsync(updateUserRequest, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task GetByToken_ShouldReturnUser_WhenDataIsValid()
        {
            //Arrange
            var jwtProvider = _scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            var userClaims = new UserClaimsDto(Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29"), "example@example.com", Role.User);
            var (accessToken, _) = jwtProvider.GenerateToken(userClaims, TokenType.Access);

            //Act
            var user = await userService.GetUserByTokenAsync(accessToken, default);

            //Assert
            user.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByToken_ShouldThrowTokenException_WhenTokenIsEmpty()
        {
            //Arrange
            var accessToken = "";

            //Act
            var exception = await Assert.ThrowsAsync<TokenException>(
                () => userService.GetUserByTokenAsync(accessToken, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Token is not valid");
        }

        [Fact]
        public async Task GetByToken_ShouldThrowNotFoundException_WhenTokenIsEmpty()
        {
            //Arrange
            var jwtProvider = _scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            var userClaims = new UserClaimsDto(Guid.NewGuid(), "example@example.com", Role.User);
            var (accessToken, _) = jwtProvider.GenerateToken(userClaims, TokenType.Access);

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => userService.GetUserByTokenAsync(accessToken, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task ConfirmSend_ShouldReturnConfirmToken_WhenDataIsValid()
        {
            //Arrange
            var callbackUrl = "callbackUrl";

            var jwtProvider = _scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            var userClaims = new UserClaimsDto(Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29"), "example@example.com", Role.User);
            var (accessToken, _) = jwtProvider.GenerateToken(userClaims, TokenType.Access);

            //Act
            var result = await userService.ConfirmEmailSendAsync(accessToken, callbackUrl, default);

            //Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ConfirmRecieve_ShouldSucceed_WhenDataIsValid()
        {
            //Arrange
            var id = Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29");
            var email = "example@example.com";
            var callbackUrl = "callbackUrl";

            var unitOfWork = _scope.ServiceProvider.GetRequiredService<IUserUnitOfWork>();
            var jwtProvider = _scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            var userClaims = new UserClaimsDto(id, email, Role.User);
            var (accessToken, _) = jwtProvider.GenerateToken(userClaims, TokenType.Access);

            var token = await userService.ConfirmEmailSendAsync(accessToken, callbackUrl, default);

            var confirmRequest = new ConfirmEmailRequest { 
                Email = email, 
                Token = token 
            };

            //Act
            await userService.ConfirmEmailRecieveAsync(confirmRequest, default);

            //Assert
            var user = await unitOfWork.UserRepository.GetByIdAsync(id, default);
            user.Should().NotBeNull();
            user.IsEmailConfirmed.Should().BeTrue();
        }


        [Fact]
        public async Task Refresh_ShouldSucceed_WhenDataIsValid()
        {
            //Arrange
            var id = Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29");
            var email = "example@example.com";

            var tokenService = _scope.ServiceProvider.GetRequiredService<ITokenService>();
            var unitOfWork = _scope.ServiceProvider.GetRequiredService<IUserUnitOfWork>();
            var jwtProvider = _scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            var refreshTokenId = Guid.NewGuid();
            var userClaims = new UserClaimsDto(id, email, Role.User);
            var (refreshToken, expiresRefresh) = jwtProvider.GenerateToken(userClaims, TokenType.Refresh, refreshTokenId);
            var token = new Token(refreshTokenId, TokenType.Refresh, userClaims.Id, refreshToken, expiresRefresh);

            await unitOfWork.TokenRepository.AddAsync(token, default);
            await unitOfWork.SaveChangesAsync(default);

            //Act
            var accessToken = await tokenService.RefreshTokenAsync(refreshToken, default);

            //Assert
            accessToken.Should().NotBeNull();
        }

        [Fact]
        public async Task Refresh_ShouldThrowTokenException_WhenDataIsValid()
        {
            //Arrange
            var refreshToken = "";

            var tokenService = _scope.ServiceProvider.GetRequiredService<ITokenService>();

            //Act
            var exception = await Assert.ThrowsAsync<TokenException>(
                () => tokenService.RefreshTokenAsync(refreshToken, default)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Token is empty");
        }
    }
}
