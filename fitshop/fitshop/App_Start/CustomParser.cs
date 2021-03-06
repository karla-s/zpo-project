﻿using fitshop.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fitshop.App_Start
{
    public static class CustomParser
    {
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

            jsonObject.id = user.id;
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
            jsonObject.author = ParseUserToJson(foods.First().user);
            jsonObject.foods = new JArray() as dynamic;

            foreach (var food in foods)
                jsonObject.foods.Add(ParseFoodToJson(food));

            return jsonObject;
        }

        public static dynamic ParseFoodToJson(food food)
        {
            dynamic jsonObject = new JObject();

            jsonObject.foodName = food.foodName;
            jsonObject.protein = food.protein;
            jsonObject.carbs = food.carbs;
            jsonObject.fat = food.fat;
            jsonObject.calories = food.calories;

            return jsonObject;
        }
    }
}