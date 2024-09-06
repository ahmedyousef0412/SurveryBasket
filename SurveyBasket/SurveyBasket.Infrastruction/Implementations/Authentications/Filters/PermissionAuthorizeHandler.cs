

namespace SurveyBasket.Infrastruction.Implementations.Authentications.Filters;
public class PermissionAuthorizeHandler : AuthorizationHandler<PermissionRequirment>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirment requirement)
    {
        var user = context.User.Identity;

        if (user is null || !user.IsAuthenticated)
            return;

        var hasPermission = context.User.Claims
            .Any(c => c.Value == requirement.Permission && c.Type == Permessions.Type);

        if (!hasPermission)
            return;

        context.Succeed(requirement);
        return;
    }
}
