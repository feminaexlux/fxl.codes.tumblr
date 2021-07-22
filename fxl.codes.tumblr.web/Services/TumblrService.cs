using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using fxl.codes.tumblr.web.Entities;
using fxl.codes.tumblr.web.Utilities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace fxl.codes.tumblr.web.Services
{
    public class TumblrService
    {
        private readonly string _accessUrl;
        private readonly string _apiKey;
        private readonly string _authorizeUrl;
        private readonly string _connectionString;
        private readonly string _requestUrl;
        private readonly string _secret;

        public TumblrService(IConfiguration configuration)
        {
            var tumblr = configuration.GetSection("Tumblr");
            _requestUrl = tumblr["RequestUrl"];
            _authorizeUrl = tumblr["AuthorizeUrl"];
            _accessUrl = tumblr["AccessUrl"];
            _apiKey = tumblr["ApiKey"];
            _secret = tumblr["ConsumerSecret"];

            _connectionString = configuration.GetConnectionString("tumblr");
        }

        internal async Task<Blog> AddBlog(string shortUrl, int userId)
        {
            var request = WebRequest.Create($"https://api.tumblr.com/v2/blog/{shortUrl}.tumblr.com/info?api_key={_apiKey}");
            var response = await request.GetResponseAsync();
            using var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
            var json = await reader.ReadToEndAsync();
            var tumblrBlogInfo = JsonSerializer.Deserialize<TumblrBlogContainer>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var blog = new Blog
            {
                Json = json,
                ShortUrl = tumblrBlogInfo.Response.Blog.Name,
                Title = tumblrBlogInfo.Response.Blog.Title,
                TumblrUuid = tumblrBlogInfo.Response.Blog.Uuid
            };
            
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var blogId = connection.QueryFirst<int>(@"insert into blogs (tumblr_uuid, title, short_url, json) values (@TumblrUuid, @Title, @ShortUrl, @Json) returning id", blog);
            await connection.ExecuteAsync("insert into user_blog (\"user\", blog) values (@User, @Blog)", new {User = userId, Blog = blogId});
            
            await connection.CloseAsync();
            return blog;
        }

        internal async Task<IEnumerable<Blog>> GetBlogs(int userId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var blogs = connection.Query<Blog>("select b.* from blogs b join user_blog ub on ub.blog = b.id and ub.user = @User", new {User = userId});
            
            await connection.CloseAsync();
            return blogs;
        }

        internal OAuth1AuthorizationContext GetAuthorizationContext()
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
    }
}