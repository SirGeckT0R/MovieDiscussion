using Microsoft.AspNetCore.Authorization;
using UserServiceDataAccess.Enums;

namespace UserServiceWebAPI.RoleAuthorization
{
    public class RoleRequirementHandler(ILogger<RoleRequirementHandler> logger) : AuthorizationHandler<RoleRequirement>
    {
        private readonly ILogger<RoleRequirementHandler> _logger = logger;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            IEnumerable<IAuthorizationRequirement> requirements = context.Requirements;

            if (!context.User.Claims.Any(x => x.Type == ClaimType.Role.ToString()))
            {
                _logger.LogError("User token has no role");

                context.Fail(new AuthorizationFailureReason(this, "User token has no role"));

                return Task.CompletedTask;
            }

            var role = context.User.FindFirst(x => x.Type == ClaimType.Role.ToString())!.Value;

            string[] roles = role.Split(',');

            string[] requireRoles = requirements.Where(y => y.GetType() == typeof(RoleRequirement)).Select(x => ((RoleRequirement)x).Role).ToArray();

            var isMatch = requireRoles.Any(x => roles.Any(y => x == y));

            if (!isMatch)
            {
                _logger.LogError("User token doesn't have the required role");

                context.Fail(new AuthorizationFailureReason(this, "User token doesn't have the required role"));

                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            _logger.LogInformation("User authorized");

            return Task.CompletedTask;
        }
    }
}
