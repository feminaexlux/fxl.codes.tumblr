using System.Collections.Generic;
using System.Linq;
using fxl.codes.tumblr.web.Utilities;
using NUnit.Framework;

namespace fxl.codes.tumblr.tests
{
    /// <summary>
    ///     Most test data taken from https://datatracker.ietf.org/doc/html/draft-hammer-oauth-10#section-3.4.1
    /// </summary>
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestOAuth1Encoding()
        {
            const string url = "http://example.net/tests";
            var encodedUrl = OAuth1.SignatureEncode(url);

            Assert.AreEqual("http%3A%2F%2Fexample.net%2Ftests", encodedUrl);

            const string parameters =
                "oauth_consumer_key=9djdj82h48djs9d2&oauth_nonce=7d8f3e4a&oauth_signature_method=HMAC-SHA1&oauth_timestamp=137131201&oauth_token=kkk9d7dh3k39sjv7";
            var encodedParameters = OAuth1.SignatureEncode(parameters);

            Assert.AreEqual("oauth_consumer_key%3D9djdj82h48djs9d2%26"
                            + "oauth_nonce%3D7d8f3e4a%26"
                            + "oauth_signature_method%3DHMAC-SHA1%26"
                            + "oauth_timestamp%3D137131201%26"
                            + "oauth_token%3Dkkk9d7dh3k39sjv7", encodedParameters);
        }

        [Test]
        public void TestOAuth1SignatureHash()
        {
            const string hash = "O3Ft1ET3b0HaopoTBz42X0n8ACM=";
            var url = OAuth1.SignatureEncode("https://www.tumblr.com/oauth/request_token");
            var dictionary = new Dictionary<string, string>
            {
                {"oauth_consumer_key", "1234"},
                {"oauth_signature_method", "HMAC-SHA1"},
                {"oauth_timestamp", "1626666646"},
                {"oauth_nonce", "12345"},
                {"oauth_version", "1.0"}
            };

            var parts = OAuth1.SignatureParts(dictionary);
            var concatenated = string.Join("&", parts.Select(x => $"{x.Key}={x.Value}"));
            var signatureBase = $"GET&{url}&{OAuth1.SignatureEncode(concatenated)}";

            var signedHash = OAuth1.SignatureHash("5678", "", signatureBase);

            Assert.AreEqual(hash, signedHash);
        }

        [Test]
        public void TestOAuth1Headers()
        {
            const string headers = "Authorization: OAuth oauth_consumer_key=\"1234\"," +
                                   "oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1626666646\",oauth_nonce=\"12345\"," +
                                   "oauth_version=\"1.0\",oauth_signature=\"O3Ft1ET3b0HaopoTBz42X0n8ACM=\"";

            var request = OAuth1.BuildRequest("GET", "https://www.tumblr.com/oauth/request_token",
                "1234",
                "5678",
                "12345",
                "1626666646");

            Assert.AreEqual(headers, request.Headers.ToString().Trim());
        }
    }
}