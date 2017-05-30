using fitshop.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fitshop.App_Start
{
    public static class CustomParser
    {
        public static Dictionary<string, string> ParseRequestBody(string requestBody)
        {
            string decodeRequestBody = HttpUtility.UrlDecode(requestBody);
            Dictionary<string, string> body = new Dictionary<string, string>();

            string[] bodyItems = decodeRequestBody.Split('&');
            foreach (var item in bodyItems)
            {
                string[] keyValue = item.Split('=');
                body.Add(keyValue.First(), keyValue.Last());
            }

            return body;
        }

        public static dynamic ParseTokenToJson(string token, double expire)
        {
            dynamic jsonObject = new JObject();
            jsonObject.token = token;
            jsonObject.token_expire = expire;

            return jsonObject;
        }

        public static dynamic ParseUserToJson(user user)
        {
            dynamic jsonObject = new JObject();

            jsonObject.login = user.login;
            jsonObject.mail = user.mail;

            return jsonObject;
        }

        public static dynamic ParseUserToJson(List<user> usersFromList)
        {
            dynamic jsonObject = new JObject();
            jsonObject.users = new JArray() as dynamic;
            foreach (var user in usersFromList)
                jsonObject.users.Add(CustomParser.ParseUserToJson(user));

            return jsonObject;
        }

        public static dynamic ParseFoodToJson(List<food> foods)
        {
            dynamic jsonObject = new JObject();
            jsonObject.foods = new JArray() as dynamic;
            foreach (var food in foods)
                jsonObject.foods.Add(ParseFoodToJson(food));

            return jsonObject;
        }

        public static dynamic ParseFoodToJson(food food)
        {
            dynamic jsonObject = new JObject();

            jsonObject.user = ParseUserToJson(food.user);
            jsonObject.foodName = food.foodName;
            jsonObject.protein = food.Protein;
            jsonObject.carbs = food.Carbs;
            jsonObject.fat = food.Fat;
            jsonObject.calories = food.Calories;

            return jsonObject;
        }
    }
}