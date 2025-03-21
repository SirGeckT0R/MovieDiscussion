using FluentAssertions;
using Grpc.Core;
using Moq;
using MovieDiscussionTests.Fakers;
using MovieGrpcClient;
using UserServiceApplication.Services;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class DeleteTest : BaseTest
    {
        public DeleteTest() : base()
        {
        }

        [Fact]
        public async Task ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var deleteRequest = Guid.NewGuid();
            var grpcReply = UserRelatedFaker.GetGrpcDeleteReply();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(deleteRequest, cancellationToken))
                .ReturnsAsync(user);
            _client.Setup(client => client.DeleteProfileAndWatchlistAsync(It.IsAny<DeleteProfileAndWatchlistRequest>(), It.IsAny<CallOptions>()))
                .Returns(grpcReply);

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
            await userService.DeleteAsync(deleteRequest, cancellationToken);

            //Assert
            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(deleteRequest, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );

            _unitOfWork.Verify(
                unit => unit.UserRepository.Delete(user, cancellationToken),
                Times.Once,
                "Delete must be called"
            );

            _unitOfWork.Verify(
                unit => unit.SaveChangesAsync(cancellationToken),
                Times.Once,
                "Save Changes must be called"
            );

            _client.Verify(
                client => client.DeleteProfileAndWatchlistAsync(It.IsAny<DeleteProfileAndWatchlistRequest>(), It.IsAny<CallOptions>()),
                Times.Once,
                "Call to grpc service must occur"
            );
        }

        [Fact]
        public async Task ShouldThrowNotFoundException_WhenNoUserFound()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var deleteRequest = Guid.NewGuid();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(deleteRequest, cancellationToken))
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
                () => userService.DeleteAsync(deleteRequest, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("User not found");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(deleteRequest, cancellationToken),
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

            var deleteRequest = Guid.NewGuid();
            var user = _userFaker.Generate();

            _unitOfWork.Setup(unit => unit.UserRepository.GetByIdAsync(deleteRequest, cancellationToken))
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
                () => userService.DeleteAsync(deleteRequest, cancellationToken)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("The operation was canceled.");

            _unitOfWork.Verify(
                unit => unit.UserRepository.GetByIdAsync(deleteRequest, cancellationToken),
                Times.Once,
                "Get by id must be called"
            );
        }
    }
}
