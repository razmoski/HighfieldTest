using HighfieldTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace HighfieldTest.Controllers
{
    public class UsersController : Controller
    {
        
        public ActionResult Index()
        {
            var url = "https://recruitment.highfieldqualifications.com/api/test";
            var UserData = GetUsersFromApi(url);
            var Ages     = GetAges(UserData);
            var Colors   = GetColours(UserData);
            
            var model = new AgeAndColours()
            {
                users      = UserData,
                ages       = Ages    ,
                topColours = Colors
            };

            MemoryCache.Default["Users"]  = UserData.ToList();
            MemoryCache.Default["Ages"]   = Ages.ToList();
            MemoryCache.Default["Colors"] = Colors.ToList();
            
            return View("CalculateResults", model);
        }

        private static List<User> GetUsersFromApi(string url)
        {
            
            var syncClient = new System.Net.WebClient();
            var content = syncClient.DownloadString(url);

            return JsonConvert.DeserializeObject<List<User>>(content);
        } 

        private static List<Age> GetAges(List<User> users)
        {
            return users.Select(a => new Age
            {
                userId        = a.id,
                originalAge   = Age(a.dob),
                agePlusTwenty = Age(a.dob) + 20
            })
            .ToList();
        }

        private static List<TopColour> GetColours(List<User> users)
        {
            return users
            .GroupBy(c => c.favouriteColour)
            .Select(x => new TopColour
                    {
                        colour = x.Key,
                        count = x.Count()
                    })
            .OrderBy(c => c.count)
            .ThenBy(c => c.colour)
            .ToList();
        }

        private static int Age(DateTime birthDate)
        {
            int age = 0;
            age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                age = age - 1;

            return age;
        }

        public ActionResult PostMyResults()
        {
            var model = new AgeAndColours()
            {
                users = MemoryCache.Default["Users"] as List<User>,
                ages = MemoryCache.Default["Ages"] as List<Age>,
                topColours = MemoryCache.Default["Colors"] as List<TopColour>
            };

            string uri = "https://recruitment.highfieldqualifications.com/api/test";

            var client = new HttpClient();

            var myJson = JsonConvert.SerializeObject(model);

            var body = new StringContent(myJson, Encoding.UTF8, "application/json");

            var response = client.PostAsync(uri, body);

            ViewBag.Status = response.Status;
            ViewBag.Result = response.Result;

            return View();
        }
    }

}