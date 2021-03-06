﻿using System;
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

        /// <summary>
        /// Returns the login view.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Login()
        {
            //Seeder.SeedProducts(_db);//used to seed db
            //Seeder.SeedStores(_db);
            return View();
        }

        /// <summary>
        /// Calls dbLoginCheck to see if the user should be logged in and stores them in the cache if they are. 
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns>If pass: Redirect to SelectLocation actionmethod</returns>
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

        /// <summary>
        /// Returns the register view.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Checks if the user should be entered into the db, and does so if true.
        /// </summary>
        /// <param name="newCust"></param>
        /// <returns>Login View</returns>
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

        /// <summary>
        /// Removes all user information from the cache and returns to the login view.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Logout()
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login");
            }

            _cache.Remove("prodIds");
            _cache.Remove("locIds");
            _cache.Remove("costs");
            _cache.Remove("prodNames");
            _cache.Remove("quantities");
            _cache.Remove("totalCost");
            _cache.Remove("custId");
            _cache.Remove("currentCust");

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Searches for the username in the db.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>View with the relevant user info.</returns>
        public IActionResult SearchUser(string username)
        {
            bool check = BusinessLogic.SearchUsername(username, _db);
            if (check)
            {
                ViewBag.userCheck = "That username exists in the database.";
                return View();
            }

            ViewBag.userCheck = "That username does not exist in the database.";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
