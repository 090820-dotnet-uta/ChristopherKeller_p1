using System;
using System.Collections.Generic;
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
    public class SelectionController : Controller
    {
        private readonly ILogger<SelectionController> _logger;
        private readonly MyDbContext _db;
        private IMemoryCache _cache;

        public SelectionController(IMemoryCache cache, MyDbContext db, ILogger<SelectionController> logger)
        {
            _logger = logger;
            _cache = cache;
            _db = db;
        }


        public IActionResult SelectLocation()
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            //bool check = true;
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }
            return View(_db.Stores);
        }

        public IActionResult ProductList(int id)
        {
            _logger.LogInformation($"Current storeID is {id}");
            //get product list
            var onlyStoreProducts = _db.Products.Where(x => x.StoreId == id).ToList();

            return View(onlyStoreProducts);
        }
    }
}
