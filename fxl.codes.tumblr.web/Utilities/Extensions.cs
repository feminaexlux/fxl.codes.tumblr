using System.Linq;
using System.Security.Claims;

namespace fxl.codes.tumblr.web.Utilities
{
    public static class Extensions
    {
        public static string GetDisplayName(this ClaimsPrincipal user)
        {
            var claims = user.Claims;
            return claims.FirstOrDefault(x => x.Type == Constants.DisplayName)?.Value ?? "";
        }
    }
}