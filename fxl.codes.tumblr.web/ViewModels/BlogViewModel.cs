using System.Collections.Generic;
using System.Linq;
using fxl.codes.tumblr.web.Entities;

namespace fxl.codes.tumblr.web.ViewModels
{
    public class BlogViewModel
    {
        public readonly Blog CurrentBlog;
        public readonly IEnumerable<Blog> AllBlogs;

        public BlogViewModel(IEnumerable<Blog> allBlogs, int? selected)
        {
            AllBlogs = allBlogs;
            CurrentBlog = selected.HasValue ? allBlogs.FirstOrDefault(x => x.Id == selected) : allBlogs.FirstOrDefault();
        }
    }
}