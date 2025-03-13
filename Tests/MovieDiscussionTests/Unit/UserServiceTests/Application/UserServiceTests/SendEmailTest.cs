using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Services;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.UserServiceTests
{
    public class SendEmailTest : BaseTest
    {
        public SendEmailTest() :base()
        {
        }

        [Fact]
        public void ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            using var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            var email = UserRelatedFaker.GetEmail();
            var token = "token";
            var title = "title";
            var callbackUrl = "callbackUrl";

            _emailService.Setup(service => service.SendEmailAsync(email, title, It.IsAny<string>(), cancellationToken));

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
            userService.SendEmail(email, token, title, callbackUrl, cancellationToken);

            //Assert
            _emailService.Verify(
                service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cancellationToken),
                Times.Once,
                "Send email must be called"
            );
        }
    }
}
