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
            if (cache.TryGetValue("currentCust", out string customer))//TO DO: add db check
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void clearCache(IMemoryCache cache)
        {
            bool check = cache.TryGetValue("currentCust", out Customer cust);
            if (check)
            {
                cache.Remove("currentCust");
            }
        }

    }
}
