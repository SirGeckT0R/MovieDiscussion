using Bogus;
using UserServiceDataAccess.Models;

namespace MovieDiscussionTests.Fakers
{
    public static class TokenRelatedFakers
    {
        public static Faker<Token> GetTokenFaker()
        {
            return new Faker<Token>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TokenValue, f => f.Random.Guid().ToString())
                .RuleFor(x => x.ExpiresAt, f => f.Date.Future());
        }
    }
}
