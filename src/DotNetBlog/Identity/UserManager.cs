using DotNetBlog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetBlog.Identity
{
    public static class UserManager
    {
        public static string GetUserName(ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            return principal.FindFirstValue(ClaimTypesConstants.Name);
        }
    }
}
