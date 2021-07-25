using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using fxl.codes.tumblr.web.Entities;
using Microsoft.Extensions.Configuration;

namespace fxl.codes.tumblr.web.Utilities
{
    internal class TumblrApi
    {
        private readonly string _apiKey;
        private readonly string _secret;
        private readonly string _requestUrl;
        private readonly string _authorizeUrl;
        private readonly string _accessUrl;
        private const string Prefix = "https://api.tumblr.com/v2";

        public TumblrApi(IConfiguration configuration)
        {
            var tumblr = configuration.GetSection("Tumblr");
            _apiKey = tumblr["ApiKey"];
            _secret = tumblr["ConsumerSecret"];
            
            _requestUrl = tumblr["RequestUrl"];
            _authorizeUrl = tumblr["AuthorizeUrl"];
            _accessUrl = tumblr["AccessUrl"];
        }

        public async Task<string> GetBlogInfoJson(Blog blog)
        {
            return await GetJson(BlogInfo(blog.TumblrUuid ?? $"{blog.ShortUrl}.tumblr.com"));
        }

        public async Task<string> GetBlogPostsJson(Blog blog, int limit = 200)
        {
            return await GetJson(BlogPosts(blog.TumblrUuid ?? $"{blog.ShortUrl}.tumblr.com", limit));
        }
        
        public OAuth1AuthorizationContext GetAuthorizationContext()
        {
            var request = OAuth1.BuildRequest(_requestUrl, _apiKey, _secret);
            var response = request.GetResponse();
            using var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
            var tokens = reader.ReadToEnd();
            var tokenDictionary = tokens.Split("&")
                .ToDictionary(x => x.Split("=")[0], x => x.Split("=")[1]);

            var oauthToken = tokenDictionary["oauth_token"];

            return new OAuth1AuthorizationContext(tokenDictionary["oauth_token"],
                tokenDictionary["oauth_token_secret"],
                $"{_authorizeUrl}?oauth_token={oauthToken}");
        }
        
        private string BlogInfo(string blog) => $"{Prefix}/blog/{blog}/info?api_key={_apiKey}";
        private string BlogPosts(string blog, int limit) => $"{Prefix}/blog/{blog}/posts?api_key={_apiKey}&notes_info=true&npf=true&limit={limit}";

        private static async Task<string> GetJson(string url)
        {
            var request = WebRequest.Create(url);
            var response = await request.GetResponseAsync();
            using var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
            return await reader.ReadToEndAsync();
        }
    }
}