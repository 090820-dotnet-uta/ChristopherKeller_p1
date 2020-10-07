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


        public IActionResult SelectLocation()//lists stores and allows user to choose one
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }
            return View(_db.Stores);
        }

        public IActionResult SetStoreId(int id)
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }
            Models.User.LocationID = id;
            var x = _db.Stores.Find(id);
            string locName = x.Location;
            TempData["locationName"] = locName;
            TempData["locationId"] = id.ToString();
            return RedirectToAction("ProductList");
        }

        public IActionResult ProductList()//displays list of products at store
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }
            int id = Models.User.LocationID;
            _logger.LogInformation($"Current storeID is {id}");
            var onlyStoreProducts = _db.Products.Where(x => x.StoreId == id).ToList();

            return View(onlyStoreProducts);
        }
        [HttpPost]
        public IActionResult ProductList(int numOrdered, int prodId)
        {
            _logger.LogInformation($"Ordered {numOrdered} of id: {prodId}");
            return RedirectToAction("AddToCart", "Cart", new { numOrdered, prodId });
            //TO DO: add check cart to product list
        }


    }
}
