using AutoMapper;
using Bogus;
using FluentValidation;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MovieDiscussionTests.Fakers;
using MovieGrpcClient;
using UserServiceApplication.Interfaces.Services;
using UserServiceApplication.Services;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class BaseTest
    {
        protected readonly Mock<IUserUnitOfWork> _unitOfWork;
        protected readonly Mock<IMapper> _mapper;
        protected readonly Mock<ITokenService> _tokenService;
        protected readonly Mock<IEmailService> _emailService;
        protected readonly Mock<IPasswordHasher> _passwordHasher;
        protected readonly Mock<IValidator<User>> _validator;
        protected readonly Mock<ILogger<UserService>> _logger;
        protected readonly Mock<MovieService.MovieServiceClient> _client;
        protected readonly Mock<IConfiguration> _configuration;
        protected readonly Mock<IBackgroundJobClient> _backgroundClient;

        protected readonly Faker<User> _userFaker;

        public BaseTest()
        {
            _unitOfWork = new Mock<IUserUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _tokenService = new Mock<ITokenService>();
            _emailService = new Mock<IEmailService>();
            _passwordHasher = new Mock<IPasswordHasher>();
            _validator = new Mock<IValidator<User>>();
            _logger = new Mock<ILogger<UserService>>();
            _client = new Mock<MovieService.MovieServiceClient>();
            _configuration = new Mock<IConfiguration>();
            _backgroundClient = new Mock<IBackgroundJobClient>();

            _userFaker = UserRelatedFaker.GetUserFaker();
        }
    }
}
