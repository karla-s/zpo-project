using fitshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace fitshop.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorization : AuthorizeAttribute
    {
        private fitshopEntities _db; // my entity  

        public CustomAuthorization() : base()
        {
            _db = new fitshopEntities();
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
                return true;

            return false;
        }

        private bool AuthorizeRequest(HttpActionContext actionContext)
        {
            if (Roles == null && Users == null || Roles == null)
                return _checkToken(actionContext);

            if (actionContext.Request.Headers.Authorization == null || actionContext.Request.Headers.Authorization.Scheme.ToString() == "")
            {
                HandleUnauthorizedRequest(actionContext);
                return false;
            }

            if (!_checkToken(actionContext))
                return false;

            if (_checkRoles(actionContext))
                return true;

            return false;
        }

        private bool _checkToken(HttpActionContext actionContext)
        {
            string requestToken = actionContext.Request.Headers.Authorization.Scheme.ToString();
            var token = _db.token.Where(x => x.tokenValue == requestToken).ToList();

            if (token.Count <= 0 || token.First().tokenValue != requestToken)
                return false;

            if (token.First().active == false || token.First().expire < DateTime.Now)
                return false;
            
            return true;
        }

        private bool _checkRoles(HttpActionContext actionContext)
        {
            List<token> tokens = _db.token.Where(x => x.tokenValue == actionContext.Request.Headers.Authorization.Scheme).ToList();
            List<userRoles> userRoles = null;

            if (tokens.Count != 1)
                return false;

            userRoles = tokens.First().user.userRoles.ToList();

            string[] rolesFromAuthorization = Roles.Split(',');

            foreach (var roles in rolesFromAuthorization)
                if (userRoles.Exists(x => x.roles.name == roles))
                    return true;

            return false;
        }
    }
}