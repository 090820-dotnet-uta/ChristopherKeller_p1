using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P1.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Caching.Memory;

namespace P1.Models
{
    public class BusinessLogic
    {

        public static bool dbLoginCheck(string usern, string passw, MyDbContext _db)
        {

            if (_db.Customers.Any(x => x.Username == usern))
            {
                var currentuser = _db.Customers.Where(x => x.Username == usern).FirstOrDefault();
                if (currentuser.Password == passw)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public static bool dbRegistrationCheck(Customer newCust, MyDbContext _db)
        {
            if (_db.Customers.Any(x => x.Username == newCust.Username))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void addNewCustomer(Customer newCust, MyDbContext _db)
        {
            _db.Customers.Add(newCust);
            _db.SaveChanges();
        }

        public static bool checkUserCache(IMemoryCache cache)
        {
            if (cache.TryGetValue("currentCust", out string customer))
            {
                if (Models.User.Username == customer)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        public static void clearNameCache(IMemoryCache cache)
        {
            bool check = cache.TryGetValue("currentCust", out Customer cust);
            if (check)
            {
                cache.Remove("currentCust");
            }
        }

        public static Cart NewCart()
        {
            Cart currentCart = new Cart();//creating a new cart and initializing accossiated lists
            List<int> prodList = new List<int>();
            currentCart.ProdIds = prodList;
            List<int> locList = new List<int>();
            currentCart.LocIds = locList;
            List<double> costList = new List<double>();
            currentCart.Costs = costList;
            List<string> prodNames = new List<string>();
            currentCart.ProdNames = prodNames;
            List<int> orderQuantity = new List<int>();
            currentCart.OrderQuantity = orderQuantity;
            return currentCart;
        }

        public static double GetCartTotal(List<int> quantities, List<double> costs)
        {
            double totalCost = 0;
            for (int j = 0; j < quantities.Count; j++)
            {
                totalCost += (costs[j] * quantities[j]);
            }
            return Math.Round(totalCost, 2);

        }

        public static int GetCustId(string username, MyDbContext _db)
        {
            try
            {
                var x = _db.Customers.Where(x => x.Username == username).FirstOrDefault();
                int id = x.CustomerId;
                return id;
            }
            catch (NullReferenceException)
            {
                var x = _db.Customers.Where(x => x.Username == User.Username).FirstOrDefault();
                int id = x.CustomerId;
                return id;
            }
        }

        public static List<string> GetLocationNameList(List<int> locIds, MyDbContext _db)
        {
            List<string> locations = new List<string>();
            foreach (var x in locIds)
            {
                var loc = _db.Stores.Find(x);
                locations.Add(loc.Location);
            }
            return locations;
        }

        public static void AddCartToDb(Cart currentCart, MyDbContext _db)
        {
            var orderList = new List<Order>();
            int currCartId;
            if (!_db.Orders.Any())//if this is the first order in the DB, set cart ID to 1
            {
                currCartId = 1;
            }
            else
            {
                var prevMaxCartId = _db.Orders.Select(x => x.CartId).ToList();//get the next available cartID
                currCartId = prevMaxCartId.Max() + 1;
            }
            for (int i = 0; i < currentCart.ProdIds.Count(); i++)
            {
                for (int j = 0; j < currentCart.OrderQuantity[i]; j++)//adds a new Order with all properties for each quantity purchased
                {
                    orderList.Add(new Order { CustomerId = currentCart.CustId, LocationId = currentCart.LocIds[i], ProductId = currentCart.ProdIds[i], Price = currentCart.Costs[i], CheckoutTime = DateTime.Now, CartId = currCartId });
                    var x = _db.Products.Find(currentCart.ProdIds[i]);
                    x.Quantity--;//subtracting out the quantity in inventory for each item purchased
                    _db.SaveChanges();
                }
            }

            _db.AddRange(orderList);
            _db.SaveChanges();
            return;
        }

        public static bool SearchUsername(string username, MyDbContext _db)
        {
            if (_db.Customers.Any(x => x.Username == username))
            {
                return true;
            }
            else return false;
        }

        public static void RemoveFromCart(int id, IMemoryCache _cache)
        {
            List<int> prodIds = (List<int>)_cache.Get("prodIds");
            List<int> locIds = (List<int>)_cache.Get("locIds");
            List<double> costs = (List<double>)_cache.Get("costs");
            List<string> prodNames = (List<string>)_cache.Get("prodNames");
            List<int> quantities = (List<int>)_cache.Get("quantities");

            int index = prodIds.IndexOf(id);

            prodIds.RemoveAt(index);
            locIds.RemoveAt(index);
            costs.RemoveAt(index);
            prodNames.RemoveAt(index);
            quantities.RemoveAt(index);

            _cache.Set("prodIds", prodIds);
            _cache.Set("locIds", locIds);
            _cache.Set("costs", costs);
            _cache.Set("prodNames", prodNames);
            _cache.Set("quantities", quantities);

            double totalCost = BusinessLogic.GetCartTotal(quantities, costs);
            _cache.Set("totalCost", totalCost);

            return;
        }

        public static bool CheckInventory(int ordered, int prodId, MyDbContext _db)
        {
            var inventory = _db.Products.Find(prodId);
            if (inventory.Quantity < ordered)
            {
                return false;
            }
            else return true;
        }

        public static double LocationRevanue(int locId, MyDbContext _db) 
        {
            double revanue = 0;
            var locOrders = _db.Orders.Where(x => x.LocationId == locId).ToList();
            foreach (var ord in locOrders) 
            {
                revanue += ord.Price;
            }
            return revanue;
        }

        public static double UserOrderTotal(int custId, MyDbContext _db) 
        {
            double total = 0;
            var userOrders = _db.Orders.Where(x => x.CustomerId == custId).ToList();
            foreach (var ord in userOrders)
            {
                total += ord.Price;
            }
            return total;
        }

        public static List<HistorySubcart> FormatLocOrderHistory(int storeId, MyDbContext _db)
        {

            List<HistorySubcart> locSubCartList = new List<HistorySubcart>();
            var locOrders = _db.Orders.Where(x => x.LocationId == storeId).ToList();
            var listofCarts = locOrders.Select(x => x.CartId).Distinct().ToList();
            foreach (var cart in listofCarts)
            {
                var cartOrders = locOrders.Where(y => y.CartId == cart).ToList();
                List<int> distCount = new List<int>();
                foreach (var cartOrder in cartOrders)
                {
                    if (distCount.Contains(cartOrder.ProductId))
                    {
                        HistorySubcart existingSubCart = locSubCartList.Where(x => x.prodId == cartOrder.ProductId).FirstOrDefault();
                        existingSubCart.quantity = existingSubCart.quantity + 1;
                    }
                    else
                    {
                        HistorySubcart newSubCart = new HistorySubcart();
                        newSubCart.cartId = cart;
                        newSubCart.prodCost = cartOrder.Price;
                        newSubCart.prodId = cartOrder.ProductId;
                        newSubCart.locId = cartOrder.LocationId;
                        newSubCart.time = cartOrder.CheckoutTime;
                        newSubCart.quantity = 1;
                        distCount.Add(cartOrder.ProductId);
                        locSubCartList.Add(newSubCart);
                    }

                }
            }
            foreach (var cart in locSubCartList)
            {
                int prodId = cart.prodId;
                cart.prodName = _db.Products.Find(prodId).Name;
            }
            return locSubCartList;
        }

        public static List<HistorySubcart> FormatUserOrderHistory(int custId, MyDbContext _db)
        {
            List<HistorySubcart> UserSubCartList = new List<HistorySubcart>();
            var custOrders = _db.Orders.Where(x => x.CustomerId == custId).ToList();
            var listofCarts = custOrders.Select(x => x.CartId).Distinct().ToList();
            foreach (var cart in listofCarts)
            {
                var cartOrders = custOrders.Where(y => y.CartId == cart).ToList();
                List<int> distCount = new List<int>();
                foreach (var cartOrder in cartOrders)
                {
                    if (distCount.Contains(cartOrder.ProductId))
                    {
                        HistorySubcart existingSubCart = UserSubCartList.Where(x => x.prodId == cartOrder.ProductId).FirstOrDefault();
                        existingSubCart.quantity = existingSubCart.quantity + 1;
                    }
                    else
                    {
                        HistorySubcart newSubCart = new HistorySubcart();
                        newSubCart.cartId = cart;
                        newSubCart.prodCost = cartOrder.Price;
                        newSubCart.prodId = cartOrder.ProductId;
                        newSubCart.locId = cartOrder.LocationId;
                        newSubCart.time = cartOrder.CheckoutTime;
                        newSubCart.quantity = 1;
                        distCount.Add(cartOrder.ProductId);
                        UserSubCartList.Add(newSubCart);
                    }

                }
            }
            foreach (var cart in UserSubCartList)
            {
                int prodId = cart.prodId;
                cart.prodName = _db.Products.Find(prodId).Name;
            }
            return UserSubCartList;
        }

    }
}
