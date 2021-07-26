using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string _connectionString;
        private readonly TumblrApi _tumblrApi;

        public TumblrService(IConfiguration configuration)
        {
            _tumblrApi = new TumblrApi(configuration);
            _connectionString = configuration.GetConnectionString("tumblr");
        }

        internal async Task<Blog> AddBlog(string shortUrl, int userId)
        {
            var json = await _tumblrApi.GetBlogInfoJson(new Blog {ShortUrl = shortUrl});
            var container = json.DeserializeTo<TumblrBlogContainer>();

            var blog = new Blog
            {
                Json = container.Response.Blog.Serialize(),
                ShortUrl = container.Response.Blog.Name,
                Title = container.Response.Blog.Title,
                TumblrUuid = container.Response.Blog.Uuid
            };

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var blogId = connection.QueryFirst<int>("insert into blogs (tumblr_uuid, title, short_url, json) values (@TumblrUuid, @Title, @ShortUrl, @Json) returning id", blog);
            await connection.ExecuteAsync("insert into user_blog (user_id, blog_id) values (@User, @Blog)", new {User = userId, Blog = blogId});

            await connection.CloseAsync();

            blog.Id = blogId;
            return blog;
        }

        internal async Task<IEnumerable<Blog>> GetBlogs(int userId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var blogs = connection.Query<Blog>("select b.* from blogs b join user_blog ub on ub.blog_id = b.id and ub.user_id = @User", new {User = userId});

            await connection.CloseAsync();
            return blogs;
        }

        internal async Task<IEnumerable<Post>> GetPosts(int blogId, int limit = 200, bool forceUpdate = false)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var blog = connection.QuerySingle<Blog>("select * from blogs where id = @Id", new {Id = blogId});
            var posts = await connection.QueryAsync<Post>("select * from posts where blog_id = @Id", new {Id = blogId});

            if (!posts.Any() || !blog.LastUpdated.HasValue || forceUpdate)
            {
                var postsJson = await _tumblrApi.GetBlogPostsJson(blog, limit);
                var container = postsJson.DeserializeTo<TumblrPostContainer>();

                blog.Title = container.Response.Blog.Title;
                blog.ShortUrl = container.Response.Blog.Name;
                blog.LastUpdated = DateTime.UtcNow;
                blog.Json = container.Response.Blog.Serialize();

                await connection.ExecuteAsync("update blogs set title=@Title, short_url=@ShortUrl, last_updated=@LastUpdated where id=@Id", blog);
                await connection.ExecuteAsync("delete from posts where blog_id=@Id", blog);

                posts = container.Response.Posts.Select(x => x.ToPost(blog));
                var next = container.Response._Links?.Next?.Href;
                
                while (!string.IsNullOrEmpty(next))
                {
                    var additionalJson = await _tumblrApi.GetNext(next);
                    var nextContainer = additionalJson.DeserializeTo<TumblrPostContainer>();
                    posts = posts.Union(nextContainer.Response.Posts.Select(x => x.ToPost(blog)));
                    next = nextContainer.Response._Links?.Next?.Href;
                }

                await connection.ExecuteAsync("insert into posts (blog_id, tumblr_id, slug, summary, json, \"timestamp\") values (@Blog, @TumblrId, @Slug, @Summary, @Json, @Timestamp)", posts);
            }

            foreach (var post in posts)
            {
                post.Parent = blog;
            }

            await connection.CloseAsync();
            return posts;
        }
    }
}