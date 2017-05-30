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
        public IHttpActionResult Register()
        {
            Dictionary<string, string> body = CustomParser.ParseRequestBody(_getBody());

            if (!body.Keys.Contains("login") || !body.Keys.Contains("password") || !body.Keys.Contains("confirmPassword") || !body.Keys.Contains("mail"))
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid parameter or parameters"));

            string login = body["login"];
            string password = Auth.HashPassword(body["password"]);
            string confirmPassword = Auth.HashPassword(body["confirmPassword"]);
            string mail = body["mail"];

            List<user> existsLoginOrMail = _db.user.Where(x => x.login == login || x.mail == mail).ToList();

            if (existsLoginOrMail.Count > 0)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Login or Mail exists"));

            if (password != confirmPassword)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Password and confirm password is not same"));



            roles roles = _db.roles.Where(x => x.name == "user").First();

            user newUser = new user() { login = login, mail = mail, password = password };
            _db.user.Add(newUser);
            _db.SaveChanges();

            user addedUser = _db.user.Where(x => x.login == login && x.mail == mail).First();
            userRoles newUserRoles = new userRoles() { userId = addedUser.id, rolesId = roles.id };

            _db.userRoles.Add(newUserRoles);
            _db.SaveChanges();

            userRoles addedUserRoles = _db.userRoles.Where(x => x.userId == addedUser.id && x.rolesId == roles.id).First();
            addedUser.rolesId = addedUserRoles.id;

            _db.user.Attach(addedUser);
            _db.Entry(addedUser).State = EntityState.Modified;
            _db.SaveChanges();


            return Ok();
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

        [CustomAuthorization(Roles = "admin")]
        [Route("login")]
        [HttpGet]
        public IHttpActionResult Login()
        {
            return Ok();
        }

        [AllowAnonymous]
        [Route("token")]
        [HttpPost]
        public IHttpActionResult Token()
        {
            string requestBody = _getBody();

            Dictionary<string, string> body = CustomParser.ParseRequestBody(requestBody);

            if (!body.Keys.Contains("login") || !body.Keys.Contains("password"))
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid parameter or parameters"));

            string login = body["login"];
            string password = Auth.HashPassword(body["password"]);

            List<user> currentUser = _db.user.Where(x => x.login == login && x.password == password).ToList();

            if (currentUser.Count != 1)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid login or password"));

            string token = Auth.GenerateToken();

            token newToken = new token() { active = true, expire = DateTime.Now + _tokenExpire, tokenValue = token, user = currentUser.First() };
            _db.token.Add(newToken);
            _db.SaveChanges();

            dynamic JsonObject = CustomParser.ParseTokenToJson(token, _tokenExpire.TotalSeconds);

            return Ok(JsonObject);
        }

        private string _getBody()
        {
            Task<string> content = ActionContext.Request.Content.ReadAsStringAsync();

            return content.Result;
        }
    }
}
