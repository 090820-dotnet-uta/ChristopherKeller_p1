using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using P1.Models;
using P1.Data;
using Microsoft.Extensions.Caching.Memory;

namespace P1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _db;
        private IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, MyDbContext db, IMemoryCache cache)
        {
            _logger = logger;
            _db = db;
            _cache = cache;
        }

        public IActionResult Login()
        {
            //Seeder.SeedProducts(_db);
            //Seeder.SeedStores(_db);
            //BusinessLogic.clearCache(_cache);
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

        public IActionResult LoginCheck(string Username, string Password)
        {
            bool check = BusinessLogic.dbLoginCheck(Username, Password, _db);
            if (check)
            {
                _cache.Set("currentCust", Username);
                string test = (string)_cache.Get("currentCust");
                _logger.LogInformation($"{test} is in the cache");
                Models.User.Username = Username;
                return RedirectToAction("SelectLocation", "Selection");

            }
            ModelState.AddModelError("Password", "Invalid Username/Password");
            _logger.LogInformation("invalid username/password combination");
            return View("Login");

        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterToDb(Customer newCust)
        {
            bool check = BusinessLogic.dbRegistrationCheck(newCust, _db);
            if (check)//passes dbcheck
            {
                _logger.LogInformation("Adding customer to db");
                BusinessLogic.addNewCustomer(newCust, _db);
                return View("Login");
            }
            else
            {
                _logger.LogInformation("customer already exists in db");
                ModelState.AddModelError("Username", "That username is already taken");
                return View("Register");
            }

        }
    }
}
