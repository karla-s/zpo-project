using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace fitshop.Providers
{
    public class FacebookProvider
    {
        private const string _ID_CLIENT = "1916555588621849";
        private const string _CLIENT_SECRET = "0f84d380a91606bd4a90ee5c59b7f613";

        private string _redirectUri = "";
        private string _responseType;
        private string _grantType;
        private string _scope;

        public FacebookProvider(string redirectUri, string responseType = "code", string grantType = "client_credentials", string scope = "email")
        {
            _redirectUri = redirectUri ?? "http://" + HttpContext.Current.Request.Url.Authority.ToString() + "/api/account/fb";
            _responseType = responseType;
            _grantType = grantType;
            _scope = scope;
        }

        public Uri GetLoginUrl()
        {
            FacebookClient client = new FacebookClient();
            var url = client.GetLoginUrl(new
            {
                client_id = _ID_CLIENT,
                client_secret = _CLIENT_SECRET,
                redirect_uri = _redirectUri,
                response_type = _responseType,
                grant_type = _grantType,
                scope = _scope
            });

            return url;
        }

        public object GetToken(string code)
        {
            FacebookClient client = new FacebookClient();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("client_id", _ID_CLIENT);
            parameters.Add("client_secret", _CLIENT_SECRET);
            parameters.Add("redirect_uri", _redirectUri);
            parameters.Add("code", code);

            return client.Get("/oauth/access_token", parameters);
        }
    }
}