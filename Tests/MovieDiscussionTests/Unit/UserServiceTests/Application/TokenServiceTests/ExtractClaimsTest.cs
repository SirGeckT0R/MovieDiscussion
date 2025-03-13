using FluentAssertions;
using Moq;
using MovieDiscussionTests.Fakers;
using UserServiceApplication.Services;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;

namespace MovieDiscussionTests.Unit.UserServiceTests.Application.TokenServiceTests
{
    public class ExtractClaimsTest : BaseTest
    {
        public ExtractClaimsTest() : base()
        {
        }

        [Fact]
        public void ShouldSucceed_WhenPassingValidData()
        {
            //Arrange
            var token = "inputToken";
            var returnTokenId = Guid.NewGuid();
            var claimPrincipal = UserRelatedFaker.GetUserPrincipal(id: returnTokenId);

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(token))
                .Returns(claimPrincipal);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );

            //Act
            var result = tokenService.ExtractClaims(token);

            //Assert
            result.Should().NotBeNull("Result must be returned");
            result.Item1.Should().Be(returnTokenId, "Token Id must be returned");
            result.Item2.Should().NotBeNull("Claims must be returned");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(token),
                Times.Once,
                "Get Principal from token must be called"
            );
        }

        [Theory]
        [InlineData("", "7d2312df-4929-4072-aa38-3b4be997e780", Role.User)]
        [InlineData("email@email.com", "", Role.User)]
        [InlineData("", "", Role.User)]
        [InlineData("", "7d2312df-4929-4072-aa38-3b4be997e780")]
        [InlineData("email@email.com", "")]
        [InlineData("", "")]
        public void ShouldThrowTokenException_WhenUserClaimsAreEmpty(string email, string userId, Role? role = null)
        {
            //Arrange
            var token = "inputToken";
            var returnTokenId = Guid.NewGuid();
            var claimPrincipal = UserRelatedFaker.GetUserPrincipal(email, returnTokenId, userId, role);

            _jwtProvider.Setup(jwt => jwt.GetPrincipalFromToken(token))
                .Returns(claimPrincipal);

            var tokenService = new TokenService(
                                               _unitOfWork.Object,
                                               _validator.Object,
                                               _jwtProvider.Object,
                                               _logger.Object
                                               );
            
            //Act
            var exception = Assert.Throws<TokenException>(
                () => tokenService.ExtractClaims(token)
            );

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Invalid token");

            _jwtProvider.Verify(
                jwt => jwt.GetPrincipalFromToken(token),
                Times.Once,
                "Get Principal from token must be called"
            );
        }
    }
}
