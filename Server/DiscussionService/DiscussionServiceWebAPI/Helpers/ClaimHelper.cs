using DiscussionServiceDomain.Exceptions;
using System.Security.Claims;

namespace DiscussionServiceWebAPI.Helpers
{
    public static class ClaimHelper 
    {
        public static Guid GetAccountIdFromUser(ClaimsPrincipal user)
        {
            var claimValue = user.FindFirst("AccountId")?.Value;

            Guid resultId;
            var isCorrectGuid = Guid.TryParse(claimValue, out resultId);

            if (!isCorrectGuid)
            {
                throw new BadRequestException("Account Id is not valid");
            }

            return resultId;
        }
    }
}
