using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Attributes;

/// <summary>
/// Allows the action only for signed-in users whose roles grant <paramref name="permissionName"/>.
///
/// A filter only stops an action by assigning <see cref="AuthorizationFilterContext.Result"/>.
/// Writing a redirect or status code to the response is not enough: MVC carries on and runs
/// the action anyway, which is how this attribute used to let any signed-in user perform
/// every admin operation while their browser was shown a redirect.
/// </summary>
public class InvokePermissionAttribute(string permissionName) : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public string PermissionName { get; } = permissionName;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            context.Result = new ChallengeResult();
            return;
        }

        var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
        var userId = context.HttpContext.User.GetCurrentUserId();
        if (!await permissionService.CheckUserPermissionAsync(userId, PermissionName))
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
    }
}
