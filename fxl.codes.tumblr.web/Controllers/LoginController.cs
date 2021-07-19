using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using fxl.codes.tumblr.Entities;
using fxl.codes.tumblr.Services;
using fxl.codes.tumblr.Utilities;
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
            User loggedIn;
            try
            {
                loggedIn = await _userService.FindUser(user);
                
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, loggedIn.Username),
                    new(ClaimTypes.Role, "Member"),
                    new(Constants.DisplayName, loggedIn.DisplayName)
                };
            
                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, Constants.AuthenticationScheme)));

                return Redirect("/");
            }
            catch
            {
                return NotFound("Invalid username or password");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Constants.AuthenticationScheme);
            return Redirect("/");
        }
    }
}