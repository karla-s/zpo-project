using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace fitshop.Providers
{
    public class FacebookProvider
    {
        private const string _ID_CLIENT = "1916555588621849";
        private const string _CLIENT_SECRET = "0f84d380a91606bd4a90ee5c59b7f613";
        private const string _FIELDS_USER = "?fields=id,name,email";

        private string _redirectUri;
        private string _responseType;
        private string _grantType;
        private string _scope;

        public FacebookProvider(string responseType = "code", string grantType = "client_credentials", string scope = "email,public_profile")
        {
            _redirectUri = "http://" + HttpContext.Current.Request.Url.Authority.ToString() + "/api/account/tokenfb";
            _responseType = responseType;
            _grantType = grantType;
            _scope = scope;
        }

        public Uri GetLoginUrl()
        {
            FacebookClient client = new FacebookClient();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("client_id", _ID_CLIENT);
            parameters.Add("client_secret", _CLIENT_SECRET);
            parameters.Add("redirect_uri", _redirectUri);
            parameters.Add("response_type", _responseType);
            parameters.Add("grant_type", _grantType);
            parameters.Add("scope", _scope);

            Uri url = client.GetLoginUrl(parameters);

            return url;
        }

        public Uri GetLogoutUrl(string token)
        {
            FacebookClient client = new FacebookClient();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("next", "http://" + HttpContext.Current.Request.Url.Authority.ToString());
            parameters.Add("access_token", token);

            Uri url = client.GetLogoutUrl(parameters);

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

            JObject json = JObject.Parse(client.Get("/oauth/access_token", parameters).ToString());

            return json;
        }

        public object GetUserData(string token)
        {
            FacebookClient client = new FacebookClient(token);
            object userData = client.Get("/me" + _FIELDS_USER);

            return userData;
        }
    }
}