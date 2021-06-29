using Microsoft.AspNetCore.Mvc;

namespace fxl.codes.tumblr.Controllers
{
    public class Home : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}