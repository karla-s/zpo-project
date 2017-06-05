using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace fitshop_client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //string content = "{\"author\":{\"id\":1,\"login\":\"admin\",\"mail\":\"s@s.pl\"},\"foods\":[{\"foodName\":\"cccccc\",\"protein\":10.5,\"carbs\":1.0,\"fat\":100.0,\"calories\":100.0},{\"foodName\":\"bbbbb\",\"protein\":1.0,\"carbs\":1.0,\"fat\":1.0,\"calories\":1.0}]}";
            ViewBag.Message = "Contact";

            return View();
        }

        public ActionResult Counter()
        {
            //string content = "{\"author\":{\"id\":1,\"login\":\"admin\",\"mail\":\"s@s.pl\"},\"foods\":[{\"foodName\":\"cccccc\",\"protein\":10.5,\"carbs\":1.0,\"fat\":100.0,\"calories\":100.0},{\"foodName\":\"bbbbb\",\"protein\":1.0,\"carbs\":1.0,\"fat\":1.0,\"calories\":1.0}]}";
            ViewBag.Message = "Counter";

            return View();
        }

        public ActionResult CounterTable()
        {
            var client = new RestClient("http://localhost:57450/api/food");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "EAAbPGN8NVhkBAEjanSWQDBzr5aIKMz0Ak5tdUEMxN2FDnScF3crJ5UchNXDt5OFnHEgIiOYoD5HjwkIvJI1uj7getyaBN3mOhyN3BZC1U2w25NDr7NjqFiu87FSdt4nwrz0F059AKg345KpuZAtXilZBaMH04YZD");
            IRestResponse response = client.Execute(request);
            dynamic content = JsonConvert.DeserializeObject<dynamic>(response.Content); // raw content as string

            return View(content);
        }

        public ActionResult Login()
        {
           
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            //var request = new RestRequest("food", Method.GET);
            //request.AddUrlSegment("Content-Type", "application/x-www-form-urlencoded");
            //request.AddHeader("Authorization", "F10B6B85D1A63793911124228EABA7239CB64B84F302A7256EA3F3A31049FA7D3BAA408E1D58CB1741CED8B0DEF8B5A52C829E475E0D69836D19886CEF15EBAB");

            //// execute the request
            ////IRestResponse response = client.Execute(request);
            //var content = response.Content; // raw content as string

            //object a = JsonConvert.DeserializeObject<object>(content);

            //ViewBag.a = a;
            return View();
        }
    }
}