using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using fxl.codes.tumblr.web.Entities;

namespace fxl.codes.tumblr.web.Utilities
{
    public static class Extensions
    {
        internal static readonly JsonSerializerOptions DefaultJsonOptions = new(JsonSerializerDefaults.Web);
        
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

        public static T DeserializeTo<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, DefaultJsonOptions);
        }

        public static string Serialize<T>(this T item)
        {
            return JsonSerializer.Serialize(item, DefaultJsonOptions);
        }

        public static Post ToPost(this TumblrPost tumblrPost, Blog blog)
        {
            return new()
            {
                Blog = blog.Id,
                Slug = tumblrPost.Slug,
                Summary = tumblrPost.Summary,
                Timestamp = tumblrPost.Timestamp,
                TumblrId = tumblrPost.Id,
                Json = tumblrPost.Serialize()
            };
        }
    }
}