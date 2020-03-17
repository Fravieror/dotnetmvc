using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using efecty.web.Models;
using StackExchange.Redis;
using System.Net;

namespace efecty.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {           
            ConfigurationOptions option = new ConfigurationOptions
            {
                AbortOnConnectFail = false,                
            };
            option.EndPoints.Add("cocky_diffie", 6379);
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(option);            
            int databaseNumber = 1;
            object asyncState = false;
            IDatabase db = redis.GetDatabase(databaseNumber, asyncState);
            string value = "valor guardado en ";
            if (!db.KeyExists("mykey")) {                
                db.StringSet("mykey", "redis");
            }
            ViewBag.dato1 = value + " " + db.StringGet("mykey");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
