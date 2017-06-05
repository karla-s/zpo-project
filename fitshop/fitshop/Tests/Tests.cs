using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fitshop.Tests
{
    using fitshop.App_Start;
    using fitshop.Models;
    using NUnit.Framework;
    [TestFixture]
    public class Tests
    {
        [Test]
        public void HashPasswordTest()
        {
            string password = "123456";
            string hashedPassword = Auth.HashPassword(password);

            Assert.That(Is.Equals("BA3253876AED6BC22D4A6FF53D8406C6AD864195ED144AB5C87621B6C233B548BAEAE6956DF346EC8C17F5EA10F35EE3CBC514797ED7DDD3145464E2A0BAB413", hashedPassword));
        }

        [Test]
        public void ParseFoodToJsonTest()
        {
            food food = new food()
            {
                id = 0,
                calories = 5.0,
                carbs = 4.0,
                protein = 6.6,
                foodName = "test",
                fat = 78.6,
                userId = 99
            };

            dynamic json = CustomParser.ParseFoodToJson(food);

            Assert.That(Is.Equals(food.calories, Double.Parse(json.calories.ToString())));
            Assert.That(Is.Equals(food.carbs, Double.Parse(json.carbs.ToString())));
            Assert.That(Is.Equals(food.protein, Double.Parse(json.protein.ToString())));
            Assert.That(Is.Equals(food.foodName, json.foodName.ToString()));
            Assert.That(Is.Equals(food.fat, Double.Parse(json.fat.ToString())));
        }

        [Test]
        public void ParseUserToJsonTest()
        {
            user user = new user()
            {
                id = 0,
                login = "login",
                mail = "s@s.pl"
            };

            dynamic json = CustomParser.ParseUserToJson(user);

            Assert.That(Is.Equals(user.id, int.Parse(json.id.ToString())));
            Assert.That(Is.Equals(user.login, json.login.ToString()));
            Assert.That(Is.Equals(user.mail, json.mail.ToString()));
        }

        [Test]
        public void ParseTokenToJsonTest()
        {
            string token = "5555aasdqfa54sd654a6s5d4as6d4";
            double expire = 55.5;

            dynamic json = CustomParser.ParseTokenToJson(token, expire);

            Assert.That(Is.Equals(token, json.token.ToString()));
            Assert.That(Is.Equals(expire, Double.Parse(json.token_expire.ToString())));
        }

        [Test]
        public void CacheTest()
        {
            Cache cache1 = Cache.GetInstance();
            Cache cache2 = Cache.GetInstance();

            Assert.That(Is.Equals(cache1, cache2));

            cache1.Add("key", "value");

            Assert.That(Is.Equals(cache1["key"], cache2["key"]));

            cache2.Remove("key");

            Assert.That(Is.Equals(cache1.Contains("key"), cache2.Contains("key")));
        }
    }
}