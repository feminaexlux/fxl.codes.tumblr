using System;

namespace fxl.codes.tumblr.web.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int Blog { get; set; }
        public long TumblrId { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Json { get; set; }
        public DateTime Timestamp { get; set; }
        public Blog Parent { get; set; }
        public string Link => $"https://{Parent.ShortUrl}.tumblr.com/{TumblrId}/{Slug}";
    }
}