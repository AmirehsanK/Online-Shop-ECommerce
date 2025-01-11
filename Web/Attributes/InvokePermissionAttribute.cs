using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Attributes;

public class InvokePermissionAttribute(string permissionName) : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            var userId = context.HttpContext.User.GetCurrentUserId();
            var userHasAccess = await permissionService.CheckUserPermissionAsync(userId, permissionName);
            if (!userHasAccess)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.HttpContext.Response.Redirect("/admin/access-denied");
            }
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.HttpContext.Response.Redirect("/admin/access-denied");
        }
    }
}