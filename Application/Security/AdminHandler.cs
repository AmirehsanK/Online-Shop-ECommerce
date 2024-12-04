namespace Application.Security;

using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;
using System.Threading.Tasks;

public class AdminHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
    {
        var isAdminClaim = context.User?.FindFirst("IsAdmin")?.Value;
        if (isAdminClaim != null && bool.TryParse(isAdminClaim, out var isAdmin) && isAdmin)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}