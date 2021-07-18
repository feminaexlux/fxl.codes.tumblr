using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using fxl.codes.tumblr.Entities;
using fxl.codes.tumblr.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fxl.codes.tumblr.Controllers
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
            var valid = await _userService.Validate(user);
            if (!valid)
            {
                return NotFound("Invalid username or password");
            }
            
            var claims = new List<Claim>
            {
                new Claim("user", user.Username),
                new Claim("role", "Member")
            };
            
            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

            return Redirect("/");
        }
    }
}