using fitshop.App_Start;
using fitshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public IHttpActionResult AddFood()
        {
            Dictionary<string, string> body = CustomParser.ParseRequestBody(_getBody());

            if (!body.Keys.Contains("foodName") || !body.Keys.Contains("protein") || !body.Keys.Contains("carbs") ||
                !body.Keys.Contains("fat") || !body.Keys.Contains("calories") || !body.Keys.Contains("userId"))
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid parameter or parameters"));
            try
            {
                string foodName = body["foodName"];
                double protein = Double.Parse(body["protein"]);
                double carbs = Double.Parse(body["carbs"]);
                double fat = Double.Parse(body["fat"]);
                double calories = Double.Parse(body["calories"]);
                int userId = int.Parse(body["userId"]);

                _db.food.Add(new food()
                {
                    foodName = foodName,
                    Protein = protein,
                    Carbs = carbs,
                    Fat = fat,
                    Calories = calories,
                    userId = userId
                });

                _db.SaveChanges();
            }
            catch
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid value of parameter or parameters"));
            }

            return Ok();
        }

        [CustomAuthorization(Roles = "admin")]
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult RemoveFood(int? id)
        {
            int result = 0;
            if (id != null)
            {
                List<food> foods = _db.food.Where(x => x.id == id).ToList();

                if (foods.Count != 1)
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Food not found"));

                _db.food.Remove(foods.First());
                result = _db.SaveChanges();

            }
            if (result == 1)
                return Ok();

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ""));
        }

        [CustomAuthorization(Roles = "admin")]
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult UpdateFood(int? id)
        {
            int result = 0;
            if (id != null)
            {
                List<food> foods = _db.food.Where(x => x.id == id).ToList();

                //if (foods.Count != 1)
                //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Food not found"));

                Dictionary<string, string> body = CustomParser.ParseRequestBody(_getBody());

                food updatedFood = new food() { userId = 0, Calories = 0, Carbs = 0, Fat = 0, foodName = "aaa", Protein = 0 };

                foreach (var item in body)
                {
                    var type = updatedFood.GetType().GetProperty(item.Key).GetValue(updatedFood, null);
                    updatedFood.GetType().GetProperty(item.Key).SetValue(type, item.Value);
                }


                //_db.food.Attach(foods.First());
                //_db.Entry(addedUser).State = EntityState.Modified;
                //_db.SaveChanges();

            }
            if (result == 1)
                return Ok();

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ""));
        }

        [CustomAuthorization(Roles = "admin,user")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetFood()
        {
            if (ActionContext.Request.Headers.Authorization == null || ActionContext.Request.Headers.Authorization.ToString() == "")
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization has been denied for this request."));

            string tokenFromRequest = ActionContext.Request.Headers.Authorization.ToString();

            user currentUser = Auth.GetUserFromToken(tokenFromRequest);

            List<food> foods = _db.food.Where(x => x.userId == currentUser.id).ToList();

            dynamic JsonObject = CustomParser.ParseFoodToJson(foods);

            return Ok(JsonObject);
        }

        private string _getBody()
        {
            Task<string> content = ActionContext.Request.Content.ReadAsStringAsync();

            return content.Result;
        }
    }
}
