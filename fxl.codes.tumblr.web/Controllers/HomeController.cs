using Microsoft.AspNetCore.Mvc;

namespace fxl.codes.tumblr.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}