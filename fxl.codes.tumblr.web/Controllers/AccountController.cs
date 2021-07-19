using fxl.codes.tumblr.web.Services;
using Microsoft.AspNetCore.Http;
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
            var authContext = _tumblrService.GetAuthorizationContext();
            // TODO find a better way to store these
            HttpContext.Session.SetString("token", authContext.Token);
            HttpContext.Session.SetString("secret", authContext.Secret);
            return Redirect(authContext.AuthorizationUrl);
        }

        public IActionResult Added()
        {
            HttpContext.Session.SetString("verifier", Request.Query["oauth_verifier"]);
            
            return View(model: Request.QueryString.Value);
        }
    }
}