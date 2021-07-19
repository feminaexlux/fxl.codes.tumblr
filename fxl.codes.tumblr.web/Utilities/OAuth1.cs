using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace fxl.codes.tumblr.Utilities
{
    public static class OAuth1
    {
        private const string EscapePattern = "[^A-Za-z0-9\\.\\-_~]";
        public static WebRequest BuildRequest(string requestUrl, string apiKey, string secret)
        {
            var nonce = Guid.NewGuid().ToString();
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            return BuildRequest("GET", requestUrl, apiKey, secret, nonce, timestamp);
        }
        
        public static WebRequest BuildRequest(string method, string requestUrl, string apiKey, string secret, string nonce, string timestamp)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"oauth_consumer_key", apiKey},
                {"oauth_signature_method", "HMAC-SHA1"},
                {"oauth_timestamp", timestamp},
                {"oauth_nonce", nonce},
                {"oauth_version", "1.0"}
            };
            
            var ordered = SignatureParts(dictionary);
            var parameters = ordered.Select(entry => $"{entry.Key}={entry.Value}").ToArray();
            var headers = dictionary.Select(entry => $"{entry.Key}=\"{entry.Value}\"").ToArray();
            var signatureBase = $"{method}&{SignatureEncode(requestUrl)}&{SignatureEncode(string.Join("&", parameters))}";

            var request = WebRequest.Create(new Uri(requestUrl));
            var signature = SignatureHash(secret, "", signatureBase);
            request.Headers["Authorization"] = $"OAuth {string.Join(",", headers)},oauth_signature=\"{signature}\"";
            
            return request;
        }

        public static KeyValuePair<string, string>[] SignatureParts(IDictionary<string, string> dictionary)
        {
            return dictionary.OrderBy(entry => entry.Key, StringComparer.Ordinal).ToArray();
        }
        
        public static string SignatureEncode(string input)
        {
            var matcher = new Regex(EscapePattern);
            var matches = matcher.Matches(input);
            var dictionary = new Dictionary<char, string>();

            foreach (Match match in matches)
            {
                var charValue = match.Value[0];
                if (dictionary.ContainsKey(charValue)) continue;
                dictionary.Add(charValue, Uri.HexEscape(charValue));
            }

            var clone = input + "";
            foreach (var (key, value) in dictionary)
            {
                clone = clone.Replace(key.ToString(), value);
            }

            return clone;
        }

        public static string SignatureHash(string secret, string token, string signatureBase)
        {
            var hmac = new HMACSHA1(Encoding.ASCII.GetBytes($"{SignatureEncode(secret)}&{SignatureEncode(token)}"));
            var signature = hmac.ComputeHash(Encoding.ASCII.GetBytes(signatureBase));
            return Convert.ToBase64String(signature);
        }
    }
}