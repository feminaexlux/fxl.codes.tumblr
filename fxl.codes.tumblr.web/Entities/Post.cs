using System;
using System.Text.Json.Serialization;
using fxl.codes.tumblr.web.Utilities;

namespace fxl.codes.tumblr.web.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int Blog { get; set; }
        
        [JsonConverter(typeof(LongValueConverter))]
        public long TumblrId { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }

        [JsonIgnore] public string Json { get; set; }

        public DateTime Timestamp { get; set; }
        public Blog Parent { get; set; }
        public TumblrPost Content => Json.DeserializeTo<TumblrPost>();
    }
}