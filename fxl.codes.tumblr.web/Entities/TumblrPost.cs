using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using fxl.codes.tumblr.web.Utilities;

namespace fxl.codes.tumblr.web.Entities
{
    public class TumblrPostContainer
    {
        public TumblrMeta Meta { get; set; }
        public TumblrPostResponse Response { get; set; }
    }

    public class TumblrPostResponse : TumblrBlogResponse
    {
        public IEnumerable<TumblrPost> Posts { get; set; }
        public int Total_Posts { get; set; }
        public TumblrPostResponseLinks _Links { get; set; }
    }

    public class TumblrPostResponseLinks
    {
        public TumblrPostResponseLinkNext Next { get; set; }
    }

    public class TumblrPostResponseLinkNext
    {
        public string Href { get; set; }
        public string Method { get; set; }
    }

    public class TumblrPost
    {
        public long Id { get; set; }
        public string Slug { get; set; }

        [JsonConverter(typeof(UnixSecondsConverter))]
        public DateTime Timestamp { get; set; }

        public string Summary { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<TumblrPostContent> Content { get; set; }
    }

    public class TumblrPostContent
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string SubType { get; set; }
        public string Alt_Text { get; set; }
        public IEnumerable<TumblrPostContentFormat> Formatting { get; set; }
        public IEnumerable<TumblrPostContentMedia> Media { get; set; }
    }

    public class TumblrPostContentFormat
    {
        public string Type { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Url { get; set; }
    }

    public class TumblrPostContentMedia
    {
        public string Media_Key { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
    }
}