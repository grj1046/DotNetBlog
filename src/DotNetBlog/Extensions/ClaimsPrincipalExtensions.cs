using DotNetBlog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserID(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            var strUserID = principal.FindFirstValue(ClaimTypesConstants.NameIdentifier);
            return new Guid(strUserID);
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            return principal.FindFirstValue(ClaimTypesConstants.Name);
        }
    }
}
