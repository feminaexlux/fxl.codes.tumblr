using fxl.codes.tumblr.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace fxl.codes.tumblr.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly TumblrService _tumblrService;

        public AccountController(TumblrService tumblrService)
        {
            _tumblrService = tumblrService;
        }

        public IActionResult AddTumblrAccount()
        {
            return Redirect(_tumblrService.GetAuthorizationUrl());
        }

        public IActionResult Added()
        {
            return View(model: Request.QueryString.Value);
        }
    }
}