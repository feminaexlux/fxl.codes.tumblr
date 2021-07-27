using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using fxl.codes.tumblr.web.Entities;
using fxl.codes.tumblr.web.Services;
using fxl.codes.tumblr.web.Utilities;
using fxl.codes.tumblr.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fxl.codes.tumblr.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly TumblrService _tumblrService;

        public HomeController(TumblrService tumblrService)
        {
            _tumblrService = tumblrService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            var blogs = await _tumblrService.GetBlogs(User.AsAppUser().Id);
            if (!blogs.Any())
            {
                return RedirectToAction("Add");
            }
            
            return View(new BlogViewModel(blogs, id));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Blog());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Blog blog)
        {
            var fetchedBlog = await _tumblrService.AddBlog(blog.ShortUrl, User.AsAppUser().Id);
            return RedirectToAction("Index", new { id = fetchedBlog.Id });
        }

        [HttpPost]
        public async Task<IActionResult> GetPosts(int blogId)
        {
            var posts = await _tumblrService.GetPosts(blogId);
            return Json(posts.OrderByDescending(x => x.Timestamp), Extensions.DefaultJsonOptions);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SimpleList(int blogId, string tag = null)
        {
            IEnumerable<SimplePost> simplified;
            if (string.IsNullOrEmpty(tag))
            {
                var posts = await _tumblrService.GetPosts(blogId);
                simplified = posts.Cast<SimplePost>().ToArray();
            }
            else
            {
                var posts = await _tumblrService.GetPostsByTag(blogId, tag);
                simplified = posts.Cast<SimplePost>().ToArray();
            }

            return Json(simplified.OrderBy(x => x.Timestamp), Extensions.DefaultJsonOptions);
        }
    }
}