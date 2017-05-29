using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace fitshop.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        public AccountController()
        {
           
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //IdentityResult result = await _repo.RegisterUser(userModel);

            //IHttpActionResult errorResult = GetErrorResult();

            //if (errorResult != null)
            //    return errorResult;

            return Ok();
        }
        
    }
}
