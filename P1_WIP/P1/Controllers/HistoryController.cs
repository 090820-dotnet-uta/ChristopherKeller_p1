using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P1.Data;
using P1.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography.X509Certificates;

namespace P1.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ILogger<HistoryController> _logger;
        private readonly MyDbContext _db;
        private IMemoryCache _cache;

        public HistoryController(ILogger<HistoryController> logger, MyDbContext db, IMemoryCache cache)
        {
            _logger = logger;
            _db = db;
            _cache = cache;
        }

        /// <summary>
        /// Calls method FormatLocOrderHistory to create a user-friendly view (subcart) containing relevant order information at the chosen location.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View</returns>
        public IActionResult LocOrderHistory(string id)
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            int x = -1;
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }
            try
            {
                x = int.Parse(id);//exception handling
            }
            catch (ArgumentNullException e)//if the user does not pass from the selection screen, id will be null
            {
                _logger.LogWarning("NullReference averted in LocationOrderHistory method.");
                x = Models.User.LocationID;
            }

            List<HistorySubcart> SubCarts = BusinessLogic.FormatLocOrderHistory(x, _db);
            return View(SubCarts);
        }

        /// <summary>
        /// Calls method FormatUserOrderHistory to create a user-friendly view based on the current customer's order history.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult UserOrderHistory()
        {
            bool check = BusinessLogic.checkUserCache(_cache);
            if (!check)
            {
                _logger.LogInformation("User not logged in, returning to login");
                return RedirectToAction("Login", "Home");
            }

            int custId = BusinessLogic.GetCustId(Models.User.Username, _db);
            List<HistorySubcart> SubCarts = BusinessLogic.FormatUserOrderHistory(custId, _db);
            return View(SubCarts);
        }
    }
}
