using fitshop.App_Start;
using fitshop.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Facebook;
using fitshop.Providers;

namespace fitshop.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private fitshopEntities _db;
        private TimeSpan _tokenExpire = TimeSpan.FromDays(1);

        public AccountController()
        {
            _db = new fitshopEntities();
        }

        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public IHttpActionResult Register(AccountModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<user> existsLoginOrMail = _db.user.Where(x => x.login == user.Login || x.mail == user.Mail).ToList();

            if (existsLoginOrMail.Count > 0)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Login or Mail exists"));

            using (DbContextTransaction dbtransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    user newUser = new user()
                    {
                        login = user.Login,
                        password = Auth.HashPassword(user.Password),
                        mail = user.Mail
                    };
                    _db.user.Add(newUser);
                    _db.SaveChanges();

                    roles roles = _db.roles.Where(x => x.name == "user").First();
                    userRoles newUserRoles = new userRoles()
                    {
                        userId = newUser.id,
                        rolesId = roles.id
                    };

                    _db.userRoles.Add(newUserRoles);
                    _db.SaveChanges();

                    newUser.rolesId = newUserRoles.id;

                    _db.user.Attach(newUser);
                    _db.Entry(newUser).State = EntityState.Modified;
                    _db.SaveChanges();

                    dbtransaction.Commit();
                }
                catch
                {
                    dbtransaction.Rollback();
                }

                return Ok();
            }
        }

        [CustomAuthorization(Roles = "admin")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetUser()
        {
            List<user> users = _db.user.ToList();

            dynamic JsonObject = CustomParser.ParseUserToJson(users);

            return Ok(JsonObject);
        }

        [AllowAnonymous]
        [Route("token")]
        [HttpPost]
        public IHttpActionResult Token(CreateTokenModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string hashedPassword = Auth.HashPassword(user.Password);

            List<user> currentUser = _db.user.Where(x => x.login == user.Login && x.password == hashedPassword).ToList();

            if (currentUser.Count != 1)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid login or password"));

            string token = Auth.GenerateToken();

            token newToken = new token()
            {
                active = true,
                expire = DateTime.Now + _tokenExpire,
                tokenValue = token,
                user = currentUser.First()
            };
            _db.token.Add(newToken);
            _db.SaveChanges();

            dynamic JsonObject = CustomParser.ParseTokenToJson(token, _tokenExpire.TotalSeconds);

            return Ok(JsonObject);
        }

        [CustomAuthorization(Roles = "admin,user")]
        [Route("logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            string token = ActionContext.Request.Headers.Authorization.ToString();

            token tokenFormDB = _db.token.Where(x => x.tokenValue == token).ToList().First();

            tokenFormDB.active = false;

            _db.token.Attach(tokenFormDB);
            _db.Entry(tokenFormDB).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok();
        }

        [AllowAnonymous]
        [Route("LoginFB")]
        [HttpGet]
        public IHttpActionResult LoginFB()
        {
            FacebookProvider fbProvider = new FacebookProvider(null);

            Uri url = fbProvider.GetLoginUrl();
            
            return Ok(url);
        }

        [AllowAnonymous]
        [Route("FB")]
        [HttpGet]
        public IHttpActionResult FB()
        {
            string code = HttpContext.Current.Request.Params["code"];
            FacebookProvider fbProvider = new FacebookProvider(null);

            object token = fbProvider.GetToken(code);

            return Ok(token.ToString());
        }

        private Uri _redirectUri()
        {
            var uriBuilder = new UriBuilder(Request.RequestUri);
            uriBuilder.Query = null;
            uriBuilder.Fragment = null;
            uriBuilder.Path = "api/account/fb";

            return uriBuilder.Uri;
        }
    }
}
