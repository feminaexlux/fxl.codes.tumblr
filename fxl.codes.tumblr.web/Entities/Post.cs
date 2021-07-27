using System;
using System.Text.Json.Serialization;
using fxl.codes.tumblr.web.Utilities;

namespace fxl.codes.tumblr.web.Entities
{
    public class Post : SimplePost
    {
        public int Id { get; set; }
        public int Blog { get; set; }
        
        [JsonConverter(typeof(LongValueConverter))]
        public long TumblrId { get; set; }
        public string Slug { get; set; }

        [JsonIgnore] public string Json { get; set; }

        public Blog Parent { get; set; }
        public TumblrPost Content => Json.DeserializeTo<TumblrPost>();
    }

    public class SimplePost
    {
        public string Url { get; set; }
        public string Summary { get; set; }
        public DateTime Timestamp { get; set; }
    }
}