using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P1.Models;
using P1.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Localization;
using System.Threading;

namespace P1.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _db;
        private IMemoryCache _cache;

        public CartController(ILogger<HomeController> logger, MyDbContext db, IMemoryCache cache)
        {
            _logger = logger;
            _db = db;
            _cache = cache;
        }
        /// <summary>
        /// Takes user info and adds them to the cache cart. Catches duplicate prodIds and adds them to the existing quantitiy.
        /// </summary>
        /// <param name="numOrdered"></param>
        /// <param name="prodId"></param>
        /// <returns>Redirect to SelectLocation ActionMethod</returns>
        public IActionResult AddToCart(int numOrdered, int prodId)
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }

            var cust = _db.Customers.Where(x => x.Username == Models.User.Username).FirstOrDefault();
            int custId = cust.CustomerId;
            var prod = _db.Products.Find(prodId);
            bool dupCheck = false; //check to see if the product ID is already added
            int dupIndex = 0;
            bool inventoryCheck = true;
            int prodIdExcess = -1;


            _cache.Set("custId", custId);
            if (_cache.TryGetValue("prodIds", out List<int> prodIds))
            {
                if (prodIds.Contains(prodId))
                {
                    dupCheck = true;
                    dupIndex = prodIds.IndexOf(prodId);
                }
                else
                {
                    prodIds.Add(prodId);
                    _cache.Set("prodIds", prodIds);
                }
            }
            else
            {
                prodIds = new List<int>();
                prodIds.Add(prodId);
                _cache.Set("prodIds", prodIds);
            }
            if (_cache.TryGetValue("locIds", out List<int> locIds))
            {
                if (!dupCheck)
                {
                    locIds.Add(Models.User.LocationID);
                    _cache.Set("locIds", locIds);
                }
            }
            else
            {
                locIds = new List<int>();
                locIds.Add(Models.User.LocationID);
                _cache.Set("locIds", locIds);
            }
            if (_cache.TryGetValue("costs", out List<double> costs))
            {
                if (!dupCheck)
                {
                    costs.Add(prod.Price);
                    _cache.Set("costs", costs);
                }
            }
            else
            {
                costs = new List<double>();
                costs.Add(prod.Price);
                _cache.Set("costs", costs);
            }
            if (_cache.TryGetValue("quantities", out List<int> quantities))
            {
                if (dupCheck)
                {
                    quantities[dupIndex] += numOrdered;
                    _cache.Set("quantities", quantities);
                    inventoryCheck = BusinessLogic.CheckInventory(quantities[dupIndex], prodIds[dupIndex], _db);
                    if (!inventoryCheck)
                    {
                        prodIdExcess = prodIds[dupIndex];
                        _cache.Set("prodIdExcess", prodIdExcess);
                    }

                }
                else
                {
                    quantities.Add(numOrdered);
                    _cache.Set("quantities", quantities);
                }
            }
            else
            {
                quantities = new List<int>();
                quantities.Add(numOrdered);
                _cache.Set("quantities", quantities);
            }
            if (_cache.TryGetValue("prodNames", out List<string> prodNames))
            {
                if (!dupCheck)
                {
                    prodNames.Add(prod.Name);
                    _cache.Set("prodNames", prodNames);
                }
            }
            else
            {
                prodNames = new List<string>();
                prodNames.Add(prod.Name);
                _cache.Set("prodNames", prodNames);
            }

            double totalCost = BusinessLogic.GetCartTotal(quantities, costs);
            _cache.Set("totalCost", totalCost);

            if (!inventoryCheck)
            {
                return RedirectToAction("FailedInventoryCheck");
            }
            else
            {
                return RedirectToAction("ProductList", "Selection", Models.User.LocationID);
            }
        }

        /// <summary>
        /// Gets user's cart info from cache and displays it to the user.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult ViewCart()
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }

            if (!_cache.TryGetValue("prodIds", out List<int> prodIds))
            {
                return View("EmptyCart");
            }
            if (prodIds.Count == 0)
            {
                return View("EmptyCart");
            }

            List<string> names = (List<string>)_cache.Get("prodNames");
            List<int> locIds = (List<int>)_cache.Get("locIds");
            List<string> locations = BusinessLogic.GetLocationNameList(locIds, _db);
            List<double> costs = (List<double>)_cache.Get("costs");
            List<int> quantities = (List<int>)_cache.Get("quantities");
            List<Subcart> currentCart = new List<Subcart>();

            for (int i = 0; i < locIds.Count(); i++)
            {
                Subcart subcart = new Subcart();
                subcart.Id = prodIds[i];
                subcart.prodName = names[i];
                subcart.quantity = quantities[i];
                subcart.cost = costs[i];
                subcart.location = locations[i];
                currentCart.Add(subcart);
            }

            double totalCost = (double)_cache.Get("totalCost");
            ViewBag.totalCost = totalCost;

            return View(currentCart);

        }

        /// <summary>
        /// Method to return a statement to the user if they attempt to access their cart while it is empty.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult EmptyCart()
        {
            return View();
        }

        /// <summary>
        /// Checks the user to confirm their decision to checkout.
        /// </summary>
        /// <returns>Redirect to Checkout ActionMethod</returns>
        public IActionResult ConfirmCheckout()
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }

            bool cartCheck = _cache.TryGetValue("prodIds", out List<int> prodIds);
            if (!cartCheck)
            {
                return View("EmptyCart");
            }
            double totalCost = (double)_cache.Get("totalCost");
            ViewBag.totalCost = totalCost;
            return RedirectToAction("Checkout");

        }

        /// <summary>
        /// Passes the user's cart to the AddCartToDb method and returns a confirmation screen.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Checkout()
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }

            bool cartCheck = _cache.TryGetValue("prodIds", out List<int> prodIds);
            if (!cartCheck)
            {
                return View("EmptyCart");
            }

            Cart currentCart = BusinessLogic.NewCart();
            currentCart.ProdIds = prodIds;
            currentCart.LocIds = (List<int>)_cache.Get("locIds");
            currentCart.CustId = (int)_cache.Get("custId");
            currentCart.OrderQuantity = (List<int>)_cache.Get("quantities");
            currentCart.ProdNames = (List<string>)_cache.Get("prodNames");
            currentCart.Costs = (List<double>)_cache.Get("costs");
            currentCart.TotalCost = (double)_cache.Get("totalCost");

            BusinessLogic.AddCartToDb(currentCart, _db);

            return View();
        }

        /// <summary>
        /// Clears the cache of any cart info after the user checks out.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult ClearCart()
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }

            bool cartCheck = _cache.TryGetValue("prodIds", out List<int> prodIds);
            if (!cartCheck)
            {
                return View("EmptyCart");
            }

            _cache.Remove("prodIds");
            _cache.Remove("locIds");
            _cache.Remove("costs");
            _cache.Remove("prodNames");
            _cache.Remove("quantities");
            _cache.Remove("totalCost");

            return RedirectToAction("SelectLocation", "Selection");
        }

        /// <summary>
        /// Passes prodId to RemoveFromCart method that will take a specific product out of the cart.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirect to ViewCart</returns>
        public IActionResult RemoveItem(int id)
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }
            BusinessLogic.RemoveFromCart(id, _cache);

            return RedirectToAction("ViewCart");


        }

        /// <summary>
        /// Calls RemoveFromCart whenever a user tries to add more items to the cart than exist in inventory and bypasses the data notation.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult FailedInventoryCheck()
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }
            int prodId = (int)_cache.Get("prodIdExcess");
            if (prodId == 0)
            {
                return RedirectToAction("SelectLocation", "Selection");
            }

            BusinessLogic.RemoveFromCart(prodId, _cache);
            return View();

        }
    }
}
