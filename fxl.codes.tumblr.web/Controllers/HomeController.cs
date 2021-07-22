using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fxl.codes.tumblr.web.Entities;
using fxl.codes.tumblr.web.Services;
using fxl.codes.tumblr.web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace fxl.codes.tumblr.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly TumblrService _tumblrService;

        public IEnumerable<Blog> Blogs { get; private set; }
        
        public HomeController(TumblrService tumblrService)
        {
            _tumblrService = tumblrService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            Blogs = await _tumblrService.GetBlogs(User.AsAppUser().Id);

            if (!Blogs.Any() || (id.HasValue && Blogs.All(x => x.Id != id)))
            {
                return RedirectToAction("Add");
            }
            
            return View(id.HasValue ? Blogs.FirstOrDefault(x => x.Id == id) : Blogs.FirstOrDefault());
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
    }
}