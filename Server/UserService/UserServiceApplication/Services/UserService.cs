using AutoMapper;
using UserServiceApplication.Dto;
using UserServiceApplication.Interfaces.Services;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;
using FluentValidation;

namespace UserServiceApplication.Services
{
    public class UserService(IUserUnitOfWork unitOfWork, 
                            ITokenService tokenService, 
                            IMapper mapper, 
                            IPasswordHasher passwordHasher, 
                            IValidator<User> validator) : BaseService<User>(validator), IUserService
    {
        private readonly IUserUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

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

        public async Task<(string, string)> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.UserRepository.GetByEmailAsync(loginRequest.Email, cancellationToken);

            if (candidate == null || !_passwordHasher.Verify(loginRequest.Password, candidate.Password))
            {
                throw new NotFoundException("No candidate found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var (accessToken, refreshToken) = await _tokenService.GenerateAuthTokensAsync(candidate, cancellationToken);

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
            var (accessToken, refreshToken) = await _tokenService.GenerateAuthTokensAsync(user, cancellationToken);

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
    }
}
