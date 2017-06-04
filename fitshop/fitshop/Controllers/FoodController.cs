using fitshop.App_Start;
using fitshop.App_Start.Generators;
using fitshop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace fitshop.Controllers
{
    [RoutePrefix("api/Food")]
    public class FoodController : ApiController
    {
        private fitshopEntities _db;

        public FoodController()
        {
            _db = new fitshopEntities();
        }

        [CustomAuthorization(Roles = "admin")]
        [Route("")]
        [HttpPost]
        public IHttpActionResult AddFood(FoodModel food)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            food newFood = new food()
            {
                userId = (int)food.UserId,
                foodName = food.FoodName,
                calories = (int)food.Calories,
                carbs = (int)food.Carbs,
                fat = (int)food.Fat,
                protein = (int)food.Protein
            };

            _db.food.Add(newFood);
            _db.SaveChanges();

            return Ok(newFood);
        }

        [CustomAuthorization(Roles = "admin")]
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult RemoveFood(int? id)
        {

            List<food> foods = _db.food.Where(x => x.id == id).ToList();

            if (foods.Count != 1)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Food not found"));

            _db.food.Remove(foods.First());
            int result = _db.SaveChanges();

            if (result == 1)
                return Ok();



            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ""));
        }

        [CustomAuthorization(Roles = "admin")]
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult UpdateFood([FromUri] int? id, [FromBody] FoodModel food)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<food> foods = _db.food.Where(x => x.id == id).ToList();

            if (foods.Count != 1)
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Food not found"));

            food foodFromDb = FoodModel.Update(foods.First(), food);

            _db.food.Attach(foodFromDb);
            _db.Entry(foodFromDb).State = EntityState.Modified;
            int result = _db.SaveChanges();

            if (result == 1)
                return Ok();


            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ""));
        }

        [CustomAuthorization(Roles = "admin,user")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetFood()
        {
            user currentUser = Auth.GetUserFromToken(ActionContext.Request.Headers.Authorization.ToString());

            List<food> foods = _db.food.Where(x => x.userId == currentUser.id).ToList();

            dynamic JsonObject = CustomParser.ParseFoodToJson(foods);

            return Ok(JsonObject);
        }

        [CustomAuthorization(Roles = "admin,user")]
        [Route("Generate/{format}")]
        [HttpGet]
        public IHttpActionResult Generate([FromUri]string format)
        {
            IGenerate generator = null;
            byte[] content;

            switch (format)
            {
                case "pdf":
                    {
                        generator = new GeneratePDF();
                        break;
                    }
                default:
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Invalid format"));

            }

            List<food> foods = _db.food.ToList();
            content = generator.Generate(foods);

            string pdfName = "Foods";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/" + format;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + "." + format);
            HttpContext.Current.Response.ContentType = "application/" + format;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.BinaryWrite(content);
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Close();

            return Ok();
        }
    }
}
