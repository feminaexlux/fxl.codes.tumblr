using System.Threading.Tasks;
using fxl.codes.tumblr.web.Services;
using fxl.codes.tumblr.web.Utilities;
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
        
        // GET
        public async Task<IActionResult> Index()
        {
            var user = User.AsAppUser();
            var blogs = await _tumblrService.GetBlogs(user.Id);
            
            return View(blogs);
        }
    }
}