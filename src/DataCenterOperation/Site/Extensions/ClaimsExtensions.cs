using System;
using System.Linq;
using System.Security.Claims;

namespace DataCenterOperation.Site.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return (user.Identity as ClaimsIdentity).GetUserId();
        }

        public static string GetUserId(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => "userid".Equals(c.Type))?.Value;
        }

        public static string GetUsername(this ClaimsPrincipal user)
        {
            return (user.Identity as ClaimsIdentity).GetUsername();
        }

        public static string GetUsername(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => "username".Equals(c.Type)).Value;
        }

        public static string GetUpdatedTime(this ClaimsPrincipal user)
        {
            return (user.Identity as ClaimsIdentity).GetUpdatedTime();
        }

        public static string GetUpdatedTime(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => "updatedtime".Equals(c.Type)).Value;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return (user.Identity as ClaimsIdentity).IsAdmin();
        }

        public static bool IsAdmin(this ClaimsIdentity identity)
        {
            return true.ToString().Equals(identity.Claims.FirstOrDefault(c => "isadmin".Equals(c.Type))?.Value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
