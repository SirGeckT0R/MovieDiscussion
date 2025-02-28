using AutoMapper;
using UserServiceApplication.Dto;
using UserServiceApplication.Interfaces.Services;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;
using FluentValidation;
using UserServiceDataAccess.Dto;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using UserServiceDataAccess.Enums;
using Hangfire;
using Microsoft.Extensions.Logging;
using MovieGrpcClient;
using UserServiceDataAccess.DatabaseHandlers.Specifications.UserSpecifications;

namespace UserServiceApplication.Services
{
    public class UserService(IUserUnitOfWork unitOfWork,
                            ITokenService tokenService,
                            IMapper mapper,
                            IPasswordHasher passwordHasher,
                            IValidator<User> validator,
                            IEmailService emailService,
                            ILogger<UserService> logger,
                            MovieService.MovieServiceClient client) : BaseService<User>(validator, logger), IUserService
    {
        private readonly IUserUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IEmailService _emailService = emailService;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly MovieService.MovieServiceClient _client = client;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<(string, string)> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt started for {Email}", loginRequest.Email);
            loginRequest.Email = loginRequest.Email.ToLower();

            var candidate = await _unitOfWork.UserRepository.GetByEmailAsync(loginRequest.Email, cancellationToken);

            if (candidate == null || !_passwordHasher.Verify(loginRequest.Password, candidate.Password))
            {
                _logger.LogError("Login attempt failed for {Email}: Email or password is incorrect", loginRequest.Email);

                throw new NotFoundException("Email or password is incorrect");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userClaims = _mapper.Map<UserClaimsDto>(candidate);
            var (accessToken, refreshToken) = await _tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Login attempt completed successfully for {Email}", loginRequest.Email);

            return (accessToken, refreshToken);
        }

        public async Task<(string, string)> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Register attempt started for {Email}", registerRequest.Email);

            registerRequest.Email = registerRequest.Email.ToLower();

            var candidate = await _unitOfWork.UserRepository.GetByEmailAsync(registerRequest.Email, cancellationToken);

            if (candidate != null)
            {
                _logger.LogError("Register attempt failed for {Email}: user already exists", registerRequest.Email);

                throw new ConflictException("User already exists");
            }

            registerRequest.Password = _passwordHasher.Generate(registerRequest.Password);

            cancellationToken.ThrowIfCancellationRequested();
            var user = _mapper.Map<User>(registerRequest);
            Validate(user);

            var addedUserId = await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
            user.Id = addedUserId;

            cancellationToken.ThrowIfCancellationRequested();
            var userClaims = _mapper.Map<UserClaimsDto>(user);
            var (accessToken, refreshToken) = await _tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createProfileAndWatchlistRequest = new CreateProfileAndWatchlistRequest
            {
                AccountId = addedUserId.ToString(),
                Username = user.Username
            };

            await _client.CreateProfileAndWatchlistAsync(createProfileAndWatchlistRequest);

            _logger.LogInformation("Register attempt completed successfully for {Email}", registerRequest.Email);

            return (accessToken, refreshToken);
        }

        public async Task UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update user attempt started for {Id}", updateUserRequest.Id);

            var candidate = await _unitOfWork.UserRepository.GetByIdAsync(updateUserRequest.Id, cancellationToken);

            if (candidate == null)
            {
                _logger.LogError("Update user attempt failed for {Id}: user not found", updateUserRequest.Id);

                throw new NotFoundException("User not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var user = _mapper.Map(updateUserRequest, candidate);
            _unitOfWork.UserRepository.Update(user, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Update user attempt completed successfully for {Id}", updateUserRequest.Id);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delete user attempt started for {Id}", id);

            var candidate = await _unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);

            if (candidate == null)
            {
                _logger.LogError("Delete user attempt failed for {Id}: user not found", id);

                throw new NotFoundException("User not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.UserRepository.Delete(candidate, cancellationToken);

            var deleteProfileAndWatchlistRequest = new DeleteProfileAndWatchlistRequest { AccountId = candidate.Id.ToString() };

            await _client.DeleteProfileAndWatchlistAsync(deleteProfileAndWatchlistRequest);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Delete user attempt completed successfully for {Id}", id);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get user by email attempt started for {Email}", email);
            email = email.ToLower();

            var user = await _unitOfWork.UserRepository.GetByEmailAsync(email, cancellationToken);

            if (user == null)
            {
                _logger.LogError("Get user by email attempt failed for {Email}: user not found", email);

                throw new NotFoundException("User not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userDto = _mapper.Map<UserDto>(user);

            _logger.LogInformation("Get user by email attempt completed successfuly for {Email}", email);

            return userDto;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get user by id attempt started for {Id}", id);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                _logger.LogError("Get user by id attempt failed for {Id}: user not found", id);

                throw new NotFoundException("User not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userDto = _mapper.Map<UserDto>(user);

            _logger.LogInformation("Get user by id attempt completed successfuly for {Id}", id);

            return userDto;
        }

        public async Task<Role> GetUserRoleByTokenAsync(string? accessToken, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get user role by id attempt started from token");

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                _logger.LogError("Get user role by id attempt failed: token is not valid");

                throw new TokenException("Token is not valid");
            }

            var (_, userClaims) = _tokenService.ExtractClaims(accessToken);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userClaims.Id, cancellationToken);

            if (user == null)
            {
                _logger.LogError("Get user role by id attempt failed: user not found");

                throw new NotFoundException("User not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var role = user.Role;

            _logger.LogInformation("Get user role by id attempt completed successfuly");

            return role;
        }

        public async Task<string> ConfirmEmailSendAsync(string? accessToken, string callbackUrl, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Confirm email send attempt started");

            var (confirmToken, email) = await _tokenService.GenerateTokenAndExtractEmailAsync(accessToken, TokenType.ConfirmEmail, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            confirmToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmToken));

            BackgroundJob.Enqueue(() => SendEmail(email, confirmToken, "Confirm Email", callbackUrl, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Confirm email send attempt completed successfully");

            return confirmToken;
        }

        public async Task<string> ConfirmEmailRecieveAsync(ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Confirm email recieve attempt started for {Email}", confirmEmailRequest.Email);

            var candidate = await _unitOfWork.UserRepository.GetByEmailTrackingAsync(confirmEmailRequest.Email, cancellationToken);

            if (candidate == null)
            {
                _logger.LogError("Confirm email recieve attempt failed for {Email}: user not found", confirmEmailRequest.Email);

                throw new NotFoundException("User not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var confirmToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailRequest.Token));
            await _tokenService.FindAndDeleteTokenAsync(confirmToken, TokenType.ConfirmEmail, cancellationToken);

            candidate.IsEmailConfirmed = true;

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Confirm email recieve attempt completed successfully for {Email}", confirmEmailRequest.Email);

            return confirmToken;
        }

        public async Task<string> ChangePasswordAsync(string? accessToken, string callbackUrl, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Change password attempt started");
            var (resetToken, email) = await _tokenService.GenerateTokenAndExtractEmailAsync(accessToken, TokenType.ResetPassword, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

            BackgroundJob.Enqueue(() => SendEmail(email, resetToken, "Reset Password", callbackUrl, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Change password attempt completed successfully");

            return resetToken;
        }

        public async Task<string> ForgotPasswordAsync(string email, string callbackUrl, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Forgot password attempt started");
            email = email.ToLower();

            var resetToken = await _tokenService.GenerateResetTokenAsync(email, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

            BackgroundJob.Enqueue(() => SendEmail(email, resetToken, "Reset Password", callbackUrl, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Forgot password attempt completed successfully");

            return resetToken;
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reset password attempt started for {Email}", resetPasswordRequest.Email);
            resetPasswordRequest = resetPasswordRequest with { Email = resetPasswordRequest.Email.ToLower() };

            var candidate = await _unitOfWork.UserRepository.GetByEmailTrackingAsync(resetPasswordRequest.Email, cancellationToken);

            if (candidate == null)
            {
                _logger.LogError("Reset password attempt failed for {Email}: user not found", resetPasswordRequest.Email);

                throw new NotFoundException("User not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var resetToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequest.Token));
            await _tokenService.FindAndDeleteTokenAsync(resetToken, TokenType.ResetPassword, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            candidate.Password = _passwordHasher.Generate(resetPasswordRequest.NewPassword);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Reset password attempt completed successfully for {Email}", resetPasswordRequest.Email);

            return resetToken;
        }

        public async Task<ICollection<UserDto>> GetFromCollectionAsync(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get users from collection  attempt started");

            var specification = new UsersFromCollectionSpecification(ids);
            var users = await _unitOfWork.UserRepository.GetWithSpecificationAsync(specification, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            var usersDto = _mapper.Map<ICollection<UserDto>>(users);

            _logger.LogInformation("Get users from collection attempt completed successfully");

            return usersDto;
        }

        public void SendEmail(string email, string token, string title, string callbackUrl, CancellationToken cancellationToken)
        {
            callbackUrl += $"?{nameof(email)}={email}&{nameof(token)}={token}";

            _emailService.SendEmailAsync(email, title,
                $"Please go through the link {callbackUrl}.", cancellationToken);

            _logger.LogInformation("Sending email to {Email} with title {Title}", email, title);
        }
    }
}
