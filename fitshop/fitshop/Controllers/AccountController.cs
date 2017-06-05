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
using System.Web.SessionState;

namespace fitshop.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private fitshopEntities _db;
        private TimeSpan _tokenExpire = TimeSpan.FromDays(1);
        private Cache _cache;

        public AccountController()
        {
            _db = new fitshopEntities();
            _cache = Cache.GetInstance();
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
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Register user failed!"));
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
        public IHttpActionResult Token(LoginModel user)
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
                user = currentUser.First(),
                type = 0
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

            token tokenFromDB = _db.token.Where(x => x.tokenValue == token).First();

            if (tokenFromDB.type == 1)
            {
                FacebookProvider fbProvider = new FacebookProvider();

                Uri url = fbProvider.GetLogoutUrl(tokenFromDB.tokenValue);
            }

            tokenFromDB.active = false;

            _db.token.Attach(tokenFromDB);
            _db.Entry(tokenFromDB).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok();
        }

        [AllowAnonymous]
        [Route("LoginFB")]
        [HttpGet]
        public IHttpActionResult LoginFB()
        {
            FacebookProvider fbProvider = new FacebookProvider();

            Uri url = fbProvider.GetLoginUrl();

            return Ok(url);
        }

        [AllowAnonymous]
        [Route("TokenFB")]
        [HttpGet]
        public IHttpActionResult TokenFB()
        {
            string code = HttpContext.Current.Request.Params["code"];
            FacebookProvider fbProvider = new FacebookProvider();

            dynamic respone = fbProvider.GetToken(code);

            string token = respone.access_token;
            TimeSpan expire = TimeSpan.FromSeconds(Double.Parse(respone.expires_in.ToString()));

            dynamic userDataFB = fbProvider.GetUserData(token);
            string email = userDataFB.email;

            List<user> users = _db.user.Where(x => x.mailFB == email).ToList();

            token tokenFB = new token()
            {
                tokenValue = token,
                expire = DateTime.Now + expire,
                type = 1,
                active = true
            };

            if (users.Count == 1)
            {
                tokenFB.userId = users.First().id;

                _db.token.Add(tokenFB);
                _db.SaveChanges();

                return Ok(token);
            }

            _cache.Add("tokenFB", respone);

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account not connected"));
        }

        [AllowAnonymous]
        [Route("ConnectToFB")]
        [HttpPost]
        public IHttpActionResult ConnectToFacebook(LoginModel user)
        {
            if (!_cache.Contains("tokenFB"))
                return BadRequest("First login in facebook");

            dynamic tokenFB = _cache["tokenFB"];
            _cache.Remove("tokenFB");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string hashedPassword = Auth.HashPassword(user.Password);

            List<user> users = _db.user.Where(x => x.login == user.Login && x.password == hashedPassword).ToList();

            if (users.Count != 1)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid login or password"));

            FacebookProvider fbProvider = new FacebookProvider();

            string tokenValue = tokenFB.access_token;
            TimeSpan expire = TimeSpan.FromSeconds(Double.Parse(tokenFB.expires_in.ToString()));

            dynamic userDataFB = fbProvider.GetUserData(tokenValue);
            string email = userDataFB.email;

            using (DbContextTransaction dbtransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    user userFromDB = users.First();
                    userFromDB.mailFB = email;

                    _db.user.Attach(userFromDB);
                    _db.Entry(userFromDB).State = EntityState.Modified;
                    _db.SaveChanges();

                    token newToken = new token()
                    {
                        tokenValue = tokenValue,
                        expire = DateTime.Now + expire,
                        type = 1,
                        active = true,
                        userId = userFromDB.id
                    };

                    _db.token.Add(newToken);
                    _db.SaveChanges();

                    dbtransaction.Commit();
                }
                catch
                {
                    dbtransaction.Rollback();
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Connected to Facebook failed!"));
                }
            }
            return Ok(tokenFB);
        }

        [CustomAuthorization(Roles = "admin,user")]
        [Route("LogoutFB")]
        [HttpGet]
        public IHttpActionResult LogoutFB()
        {
            string token = HttpContext.Current.Request.Params["code"];
            FacebookProvider fbProvider = new FacebookProvider();

            Uri url = fbProvider.GetLogoutUrl(token);

            return Ok(url);
        }

    }
}
