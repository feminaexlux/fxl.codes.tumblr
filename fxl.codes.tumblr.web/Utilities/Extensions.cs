using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using fxl.codes.tumblr.web.Entities;

namespace fxl.codes.tumblr.web.Utilities
{
    public static class Extensions
    {
        public static User AsAppUser(this ClaimsPrincipal user)
        {
            var claims = user.Claims.ToDictionary(x => x.Type, x => x.Value);

            return new User
            {
                Id = int.Parse(claims[ClaimTypes.PrimarySid]),
                Username = claims[ClaimTypes.NameIdentifier],
                DisplayName = claims[ClaimTypes.Name]
            };
        }

        public static IEnumerable<Claim> AsClaims(this User user)
        {
            return new[]
            {
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.DisplayName),
                // Probably needed?
                new Claim(ClaimTypes.Role, "Member")
            };
        }
    }
}