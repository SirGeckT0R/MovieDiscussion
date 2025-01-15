﻿using AutoMapper;
using UserServiceApplication.Dto;
using UserServiceApplication.Interfaces.Services;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;
using FluentValidation;
using UserServiceDataAccess.Dto;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System;

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
                throw new NotFoundException("No candidate found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userClaims = _mapper.Map<UserClaimsDto>(candidate);
            var (accessToken, refreshToken) = await _tokenService.GenerateAuthTokensAsync(userClaims, cancellationToken);

            _unitOfWork.Save();
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

            _unitOfWork.Save();

            return (accessToken, refreshToken);
        }

        public async Task UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByIdAsync(updateUserRequest.Id, cancellationToken) ?? throw new NotFoundException("No user was found");
            
            var user = _mapper.Map(updateUserRequest, candidate);
            _unitOfWork.UserRepository.Update(user, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Save();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _ = await _unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("No user found");

            await _unitOfWork.UserRepository.DeleteAsync(id, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Save();
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
            var (confirmToken, email) = await _tokenService.GenerateConfirmEmailTokenAsync(accessToken, cancellationToken);
            confirmToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmToken));
            SendEmail(email, confirmToken, "Confirm Email", callbackUrl, cancellationToken);
            _unitOfWork.Save();
            return confirmToken;
        }

        public async Task<string> ConfirmEmailRecieveAsync(ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByEmailTrackingAsync(confirmEmailRequest.Email, cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundException("No user found");
            }
            cancellationToken.ThrowIfCancellationRequested();
            var confirmToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailRequest.Token));
            await _tokenService.ValidateConfirmTokenAsync(confirmToken, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            candidate.IsEmailConfirmed = true;

            _unitOfWork.Save();
            return confirmToken;
        }

        private void SendEmail(string email, string token, string title, string callbackUrl, CancellationToken cancellationToken)
        {
            callbackUrl +=$"?{nameof(email)}={email}&{nameof(token)}={token}";

            _emailService.SendEmailAsync(email, title,
                $"Please go through the link {callbackUrl}.", cancellationToken);
        }
    }
}
