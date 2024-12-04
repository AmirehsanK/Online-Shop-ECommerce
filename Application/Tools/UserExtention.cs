using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tools
{
    public static class UserExtention
    {
        public static int GetCurrentUserId(this ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                return Convert.ToInt32(principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }
            return default;
        }
        public static int GetCurrentUserId(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;
            if (principal.Identity.IsAuthenticated)
            {
                return user.GetCurrentUserId();
            }
            return default;
        }
    }
}
