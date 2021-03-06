using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using fxl.codes.tumblr.web.Entities;
using fxl.codes.tumblr.web.Services;
using fxl.codes.tumblr.web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fxl.codes.tumblr.web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new User());
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Forbidden)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Index(User user)
        {
            var loggedIn = await _userService.FindUser(user);
            var claims = loggedIn.AsClaims();

            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, Constants.AuthenticationScheme)));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Constants.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}