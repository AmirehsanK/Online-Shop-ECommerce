using System.Security.Claims;
using System.Security.Principal;

namespace Application.Tools;

public static class UserExtention
{
    public static int GetCurrentUserId(this ClaimsPrincipal principal)
    {
        if (principal.Identity.IsAuthenticated)
            return Convert.ToInt32(principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        return default;
    }

    public static int GetCurrentUserId(this IPrincipal principal)
    {
        var user = (ClaimsPrincipal)principal;
        if (principal.Identity.IsAuthenticated) return user.GetCurrentUserId();
        return default;
    }
    public static string GetCurrentUserMobileNumber(this ClaimsPrincipal principal)
    {
        if (principal.Identity.IsAuthenticated)
            return principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;
        return string.Empty;
    }

    public static string GetCurrentUserMobileNumber(this IPrincipal principal)
    {
        var user = (ClaimsPrincipal)principal;
        if (principal.Identity.IsAuthenticated) return user.GetCurrentUserMobileNumber();
        return string.Empty;
    }

    public static string GetCurrentUserFullName(this ClaimsPrincipal principal)
    {
        if (principal.Identity.IsAuthenticated)
            return principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        return string.Empty;
    }

    public static string GetCurrentUserFullName(this IPrincipal principal)
    {
        var user = (ClaimsPrincipal)principal;
        if (principal.Identity.IsAuthenticated) return user.GetCurrentUserFullName();
        return string.Empty;
    }
}