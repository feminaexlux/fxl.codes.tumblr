using System;
using System.Collections.Generic;

namespace fxl.codes.tumblr.web.Entities
{
    public class TumblrBlogContainer
    {
        public TumblrMeta Meta { get; set; }
        public TumblrBlogInfo Response { get; set; }
    }

    public class TumblrBlogInfo
    {
        public TumblrBlog Blog { get; set; }
    }

    public class TumblrBlog
    {
        public bool Ask { get; set; }
        public bool Ask_Anon { get; set; }
        public string Ask_Page_Title { get; set; }
        public bool Asks_Allow_Media { get; set; }
        public IEnumerable<TumblrAvatar> Avatar { get; set; }
        public bool Can_Chat { get; set; }
        public bool Can_Subscribe { get; set; }
        public string Description { get; set; }
        public bool Is_Nsfw { get; set; }
        public string Name { get; set; }
        public int Posts { get; set; }
        public bool Share_Likes { get; set; }
        public string Submission_Page_Title { get; set; }
        public bool Subscribed { get; set; }
        public TumblrTheme Theme { get; set; }
        public string Title { get; set; }
        public int Total_Posts { get; set; }
        public DateTime Updated { get; set; }
        public string Url { get; set; }
        public string Uuid { get; set; }
        public bool Is_Optout_Ads { get; set; }
    }

    public class TumblrAvatar
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
    }

    public class TumblrTheme
    {
        public int Header_Full_Width { get; set; }
        public int Header_Full_Height { get; set; }
        public int Header_Focus_Width { get; set; }
        public int Header_Focus_Height { get; set; }
        public string Avatar_Shape { get; set; }
        public string Background_Color { get; set; }
        public string Body_Font { get; set; }
        public string Header_Bounds { get; set; }
        public string Header_Image { get; set; }
        public string Header_Image_Focused { get; set; }
        public string Header_Image_Poster { get; set; }
        public string Header_Image_Scaled { get; set; }
        public bool Header_Stretch { get; set; }
        public string Link_Color { get; set; }
        public bool Show_Avatar { get; set; }
        public bool Show_Description { get; set; }
        public bool Show_Header_Image { get; set; }
        public bool Show_Title { get; set; }
        public string Title_Color { get; set; }
        public string Title_Font { get; set; }
        public string Title_Font_Weight { get; set; }
    }
}