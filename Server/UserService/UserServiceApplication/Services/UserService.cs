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

namespace UserServiceApplication.Services
{
    public class UserService(IUserUnitOfWork unitOfWork, 
                            ITokenService tokenService, 
                            IMapper mapper, 
                            IPasswordHasher passwordHasher, 
                            IValidator<User> validator,
                            IEmailService emailService) : BaseService<User>(validator), IUserService
    {
        private readonly IUserUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IEmailService _emailService = emailService;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;



        public async Task<(string, string)> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByEmailAsync(loginRequest.Email, cancellationToken);

            if (candidate == null || !_passwordHasher.Verify(loginRequest.Password, candidate.Password))
            {
                throw new NotFoundException("Email or password is incorrect");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userClaims = _mapper.Map<UserClaimsDto>(candidate);
            var (accessToken, refreshToken) = await _tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken);

            await _unitOfWork.SaveAsync();

            return (accessToken, refreshToken);
        }

        public async Task<(string, string)> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByEmailAsync(registerRequest.Email, cancellationToken);
            if (candidate != null)
            {
                throw new NotFoundException("User already exists");
            }
            registerRequest.Password = _passwordHasher.Generate(registerRequest.Password);

            var user = _mapper.Map<User>(registerRequest);
            Validate(user);

            var addedUserId = await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
            user.Id = addedUserId;

            cancellationToken.ThrowIfCancellationRequested();
            var userClaims = _mapper.Map<UserClaimsDto>(user);
            var (accessToken, refreshToken) = await _tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken);

            await _unitOfWork.SaveAsync();

            return (accessToken, refreshToken);
        }

        public async Task UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByIdAsync(updateUserRequest.Id, cancellationToken) ?? throw new NotFoundException("No user was found");
            
            var user = _mapper.Map(updateUserRequest, candidate);
            _unitOfWork.UserRepository.Update(user, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("No user found");
            _unitOfWork.UserRepository.Delete(candidate, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(email, cancellationToken) ?? throw new NotFoundException("No such user found");
            cancellationToken.ThrowIfCancellationRequested();

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("No such user found");
            cancellationToken.ThrowIfCancellationRequested();

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<string> ConfirmEmailSendAsync(string? accessToken, string callbackUrl, CancellationToken cancellationToken)
        {
            var (confirmToken, email) = await _tokenService.GenerateTokenAndExtractEmailAsync(accessToken, TokenType.ConfirmEmail ,cancellationToken);
            confirmToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmToken));
            SendEmail(email, confirmToken, "Confirm Email", callbackUrl, cancellationToken);
            await _unitOfWork.SaveAsync();

            return confirmToken;
        }

        public async Task<string> ConfirmEmailRecieveAsync(ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByEmailTrackingAsync(confirmEmailRequest.Email, cancellationToken) ?? throw new NotFoundException("No user found");
            cancellationToken.ThrowIfCancellationRequested();
            var confirmToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailRequest.Token));
            await _tokenService.FindAndDeleteTokenAsync(confirmToken, TokenType.ConfirmEmail, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            candidate.IsEmailConfirmed = true;

            await _unitOfWork.SaveAsync();

            return confirmToken;
        }


        public async Task<string> ForgotPasswordAsync(string? accessToken, string callbackUrl, CancellationToken cancellationToken)
        {
            var (resetToken, email) = await _tokenService.GenerateTokenAndExtractEmailAsync(accessToken, TokenType.ResetPassword, cancellationToken);
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));
            SendEmail(email, resetToken, "Reset Password", callbackUrl, cancellationToken);
            await _unitOfWork.SaveAsync();

            return resetToken;
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByEmailTrackingAsync(resetPasswordRequest.Email, cancellationToken) ?? throw new NotFoundException("No user found");
            cancellationToken.ThrowIfCancellationRequested();
            var resetToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequest.Token));
            await _tokenService.FindAndDeleteTokenAsync(resetToken,TokenType.ResetPassword, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            candidate.Password = _passwordHasher.Generate(resetPasswordRequest.NewPassword);

            await _unitOfWork.SaveAsync();

            return resetToken;
        }

        private void SendEmail(string email, string token, string title, string callbackUrl, CancellationToken cancellationToken)
        {
            callbackUrl += $"?{nameof(email)}={email}&{nameof(token)}={token}";

            _emailService.SendEmailAsync(email, title,
                $"Please go through the link {callbackUrl}.", cancellationToken);
        }
    }
}
