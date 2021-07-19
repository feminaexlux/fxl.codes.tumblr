using System.IO;
using System.Linq;
using System.Text;
using fxl.codes.tumblr.Utilities;
using Microsoft.Extensions.Configuration;

namespace fxl.codes.tumblr.Services
{
    public class TumblrService
    {
        private readonly string _requestUrl;
        private readonly string _authorizeUrl;
        private readonly string _accessUrl;
        private readonly string _apiKey;
        private readonly string _secret;
        
        public TumblrService(IConfiguration configuration)
        {
            var tumblr = configuration.GetSection("Tumblr");
            _requestUrl = tumblr["RequestUrl"];
            _authorizeUrl = tumblr["AuthorizeUrl"];
            _accessUrl = tumblr["AccessUrl"];
            _apiKey = tumblr["ApiKey"];
            _secret = tumblr["ConsumerSecret"];
        }

        public string GetAuthorizationUrl()
        {
            var request = OAuth1.BuildRequest(_requestUrl, _apiKey, _secret);
            var response = request.GetResponse();
            using var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
            var tokens = reader.ReadToEnd();
            var tokenDictionary = tokens.Split("&")
                .ToDictionary(x => x.Split("=")[0], x => x.Split("=")[1]);

            var oauthToken = tokenDictionary["oauth_token"];

            return $"{_authorizeUrl}?oauth_token={oauthToken}";
        }
    }
}