using Bogus;
using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Services;
using UserServiceDataAccess.DatabaseHandlers.Specifications.TokenSpecifications;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class GenerateResetTokenTest : BaseTest
    {
        private readonly Faker<User> _userFaker;

        public GenerateResetTokenTest() : base()
        {
            _userFaker = UserRelatedFaker.GetUserFaker();
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var email = UserRelatedFaker.GetEmail();
            var user = _userFaker.Generate();
            var candidates = new List<Token> { _tokenFaker.Generate() };
            var returnTokenValue = "returnToken";

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken))
                .ReturnsAsync(user);
            _unitOfWork.Setup(unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken))
                .ReturnsAsync(candidates);
            _jwtProvider.Setup(jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), TokenType.ResetPassword, It.IsAny<Guid>()))
                .Returns((returnTokenValue, It.IsAny<DateTime>()));

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.GenerateResetTokenAsync(email, cancellationToken);

            //Assert
            result.Should().Be(returnTokenValue, "Result must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );


            _unitOfWork.Verify(
                unit => unit.TokenRepository.Delete(candidates[0], cancellationToken),
                Times.Once,
                "Delete must be called"
            );

            _jwtProvider.Verify(
                jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), TokenType.ResetPassword, It.IsAny<Guid>()),
                Times.Once,
                "Generate refresh token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.AddAsync(It.IsAny<Token>(), cancellationToken),
                Times.Once,
                "Add token must be called"
            );
        }

        [Fact]
        public async Task ShouldSucceed_WhenNoTokenFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var email = UserRelatedFaker.GetEmail();
            var user = _userFaker.Generate();
            var candidates = new List<Token> { };
            var returnTokenValue = "returnToken";

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken))
                .ReturnsAsync(user);
            _unitOfWork.Setup(unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken))
                .ReturnsAsync(candidates);
            _jwtProvider.Setup(jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), TokenType.ResetPassword, It.IsAny<Guid>()))
                .Returns((returnTokenValue, It.IsAny<DateTime>()));

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = await tokenService.GenerateResetTokenAsync(email, cancellationToken);

            //Assert
            result.Should().Be(returnTokenValue, "Result must be returned");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.Delete(It.IsAny<Token>(), cancellationToken),
                Times.Never,
                "Delete must not be called"
            );

            _jwtProvider.Verify(
                jwt => jwt.GenerateToken(It.IsAny<UserClaimsDto>(), TokenType.ResetPassword, It.IsAny<Guid>()),
                Times.Once,
                "Generate refresh token must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.AddAsync(It.IsAny<Token>(), cancellationToken),
                Times.Once,
                "Add token must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowBadRequestException_WhenEmailisEmpty()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var email = "";

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => tokenService.GenerateResetTokenAsync(email, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Email is not valid");
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenNoUserFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var email = UserRelatedFaker.GetEmail();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken))
                .ReturnsAsync((User?) null);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => tokenService.GenerateResetTokenAsync(email, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowInvalidOperationException_WhenTooManyCandidates()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var email = UserRelatedFaker.GetEmail();
            var user = _userFaker.Generate();
            var candidates = new List<Token> { _tokenFaker.Generate(), _tokenFaker.Generate() };

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken))
                .ReturnsAsync(user);
            _unitOfWork.Setup(unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken))
                .ReturnsAsync(candidates);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => tokenService.GenerateResetTokenAsync(email, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );

            _unitOfWork.Verify(
                unit => unit.TokenRepository.GetWithSpecificationAsync(It.IsAny<UserIdAndTypeSpecification>(), cancellationToken),
                Times.Once,
                "Get with secification must be called"
            );
        }

        [Fact]
        public async Task ShouldThrowOperationCancelledException_WhenCancelling()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            tokenSource.Cancel();

            var email = UserRelatedFaker.GetEmail();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken))
                .ReturnsAsync(user);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                () => tokenService.GenerateResetTokenAsync(email, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByEmailAsync(email, cancellationToken),
                Times.Once,
                "Get by email must be called"
            );
        }
    }
}
