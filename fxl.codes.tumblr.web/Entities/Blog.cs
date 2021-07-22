namespace fxl.codes.tumblr.web.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string TumblrUuid { get; set; }
        public string Title { get; set; }
        public string ShortUrl { get; set; }
        public string Avatar { get; set; }
        public string AvatarMime { get; set; }
        public string Json { get; set; }

        public string GetAvatarBase64()
        {
            if (string.IsNullOrEmpty(Avatar) || string.IsNullOrEmpty(AvatarMime)) return "";

            return $"data:{AvatarMime};base64,{Avatar}";
        } 
    }
}