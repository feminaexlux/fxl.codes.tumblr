using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using fxl.codes.tumblr.web.Services;
using fxl.codes.tumblr.web.Utilities;
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

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> AssociateTumblrAccount(string shortUrl)
        {
            var user = User.AsAppUser();
            var encoded = HtmlEncoder.Default.Encode(shortUrl);
            try
            {
                var blog = await _tumblrService.AddBlog(encoded, user.Id);
                return Ok(blog);
            }
            catch
            {
                return NotFound(encoded);
            }
        }

        public IActionResult VerifyTumblrAccount()
        {
            // var authContext = _tumblrService.GetAuthorizationContext();
            // // TODO find a better way to store these
            // HttpContext.Session.SetString("token", authContext.Token);
            // HttpContext.Session.SetString("secret", authContext.Secret);
            // return Redirect(authContext.AuthorizationUrl);
            return Ok();
        }

        public IActionResult Added()
        {
            HttpContext.Session.SetString("verifier", Request.Query["oauth_verifier"]);
            
            return View(model: Request.QueryString.Value);
        }
    }
}