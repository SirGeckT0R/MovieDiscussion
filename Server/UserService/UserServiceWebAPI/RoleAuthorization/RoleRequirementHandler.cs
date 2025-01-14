using Microsoft.AspNetCore.Authorization;
using UserServiceDataAccess.Enums;

namespace UserServiceWebAPI.RoleAuthorization
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            IEnumerable<IAuthorizationRequirement> requirements = context.Requirements;

            if (!context.User.Claims.Any(x => x.Type == E_ClaimType.Role.ToString()))
            {
                context.Fail(new AuthorizationFailureReason(this, "User token has no role"));
                return Task.CompletedTask;
            }

            var role = context.User.FindFirst(x => x.Type == E_ClaimType.Role.ToString())!.Value;

            string[] roles = role.Split(',');

            string[] requireRoles = requirements.Where(y => y.GetType() == typeof(RoleRequirement)).Select(x => ((RoleRequirement)x).Role).ToArray();

            var isMatch = requireRoles.Any(x => roles.Any(y => x == y));

            if (!isMatch)
            {
                context.Fail(new AuthorizationFailureReason(this, "User token doesn't has the required role"));
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
