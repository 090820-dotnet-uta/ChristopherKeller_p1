using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using P1.Models;
using P1.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace P1Tests
{
    public class UnitTests
    {
        [Fact]
        public void TestCheckUserCache()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            P1.Models.User.Username = "customer";
            memoryCache.Set("currentCust", "customer");

            bool result = P1.Models.BusinessLogic.checkUserCache(memoryCache);
            Assert.True(result);
        }
        [Fact]
        public void TestdbLoginCheck()
        {
            bool result = false;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestdbLoginCheck").Options;

            using (var context = new MyDbContext(options))
            {
                context.Customers.Add(new Customer { CustomerId = 1, FirstName = "first", LastName = "last", Password = "pass", Username = "user" });
                context.SaveChanges();
            }
            using (var context = new MyDbContext(options))
            {
                result = BusinessLogic.dbLoginCheck("user", "pass", context);
            }
            Assert.True(result);
        }
        [Fact]
        public void TestdbRegistrationCheck()
        {
            bool result = false;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestdbRegistrationCheck").Options;

            using (var context = new MyDbContext(options))
            {
                context.Customers.Add(new Customer { CustomerId = 1, FirstName = "first", LastName = "last", Password = "pass", Username = "user" });
                context.SaveChanges();
            }
            using (var context = new MyDbContext(options))
            {
                Customer newCust = new Customer();
                newCust.Username = "usern";
                result = BusinessLogic.dbRegistrationCheck(newCust, context);
                Assert.True(result);
            }
        }
        [Fact]
        public void TestAddNewCustomer()
        {
            bool result = false;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestAddNewCustomer").Options;


            using (var context = new MyDbContext(options))
            {
                Customer newCust = new Customer { CustomerId = 1, FirstName = "first", LastName = "last", Password = "pass", Username = "user" };
                BusinessLogic.addNewCustomer(newCust, context);

                if (context.Customers.Find(1).FirstName == "first") { result = true; }

                Assert.True(result);
            }
        }
        [Fact]
        public void TestRemoveNameCache()
        {
            bool result = false;
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            P1.Models.User.Username = "customer";
            memoryCache.Set("currentCust", "customer");

            BusinessLogic.clearNameCache(memoryCache);

            if ((string)memoryCache.Get("currentCust") == "customer")
            {
                result = true;
            }

            Assert.True(result);
        }
        [Fact]
        public void TestGetNewCart()
        {
            bool result = false;
            Cart newCart = BusinessLogic.NewCart();
            if (newCart.LocIds != null)
            {
                result = true;
            }
            Assert.True(result);
        }
        [Fact]
        public void TestOrderAddingToDb()
        {
            int count = -1;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestOrderAddingToDb").Options;
            using (var context = new MyDbContext(options))
            {
                var productList = new List<Product>()
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3 },
                };
                context.Products.AddRange(productList);
                context.SaveChanges();

                Cart newCart = BusinessLogic.NewCart();
                newCart.Costs.Add(100);
                newCart.Costs.Add(200);
                newCart.Costs.Add(300);
                newCart.CustId = 1;
                newCart.LocIds.Add(1);
                newCart.LocIds.Add(2);
                newCart.LocIds.Add(3);
                newCart.OrderQuantity.Add(3);
                newCart.OrderQuantity.Add(2);
                newCart.OrderQuantity.Add(1);
                newCart.ProdIds.Add(1);
                newCart.ProdIds.Add(2);
                newCart.ProdIds.Add(3);
                newCart.ProdNames.Add("a");
                newCart.ProdNames.Add("b");
                newCart.ProdNames.Add("c");
                newCart.TotalCost = 1000.00;

                BusinessLogic.AddCartToDb(newCart, context);
                count = context.Orders.Count();
            }
            Assert.Equal(6, count);
        }
        [Fact]
        public void TestDbDecrement()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestDbDecrement").Options;
            using (var context = new MyDbContext(options))
            {
                var productList = new List<Product>()
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3 },
                };
                context.Products.AddRange(productList);
                context.SaveChanges();

                Cart newCart = BusinessLogic.NewCart();
                newCart.Costs.Add(100);
                newCart.Costs.Add(200);
                newCart.Costs.Add(300);
                newCart.CustId = 1;
                newCart.LocIds.Add(1);
                newCart.LocIds.Add(2);
                newCart.LocIds.Add(3);
                newCart.OrderQuantity.Add(3);
                newCart.OrderQuantity.Add(2);
                newCart.OrderQuantity.Add(1);
                newCart.ProdIds.Add(1);
                newCart.ProdIds.Add(2);
                newCart.ProdIds.Add(3);
                newCart.ProdNames.Add("a");
                newCart.ProdNames.Add("b");
                newCart.ProdNames.Add("c");
                newCart.TotalCost = 1000.00;

                BusinessLogic.AddCartToDb(newCart, context);
                int ans = context.Products.Find(1).Quantity;

                Assert.Equal(47, ans);
            }
        }
        [Fact]
        public void TestGetCartTotal()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestGetCartTotal").Options;
            using (var context = new MyDbContext(options))
            {
                var productList = new List<Product>()
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3 },
                };
                context.Products.AddRange(productList);
                context.SaveChanges();

                Cart newCart = BusinessLogic.NewCart();
                newCart.Costs.Add(100);
                newCart.Costs.Add(200);
                newCart.Costs.Add(300);
                newCart.CustId = 1;
                newCart.LocIds.Add(1);
                newCart.LocIds.Add(2);
                newCart.LocIds.Add(3);
                newCart.OrderQuantity.Add(3);
                newCart.OrderQuantity.Add(2);
                newCart.OrderQuantity.Add(1);
                newCart.ProdIds.Add(1);
                newCart.ProdIds.Add(2);
                newCart.ProdIds.Add(3);
                newCart.ProdNames.Add("a");
                newCart.ProdNames.Add("b");
                newCart.ProdNames.Add("c");

                double ans = BusinessLogic.GetCartTotal(newCart.OrderQuantity, newCart.Costs);
                Assert.Equal(1000.00, ans);

            }
        }
        [Fact]
        public void TestGetCustId()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestGetCustId").Options;
            using (var context = new MyDbContext(options))
            {
                context.Customers.Add(new Customer { CustomerId = 1, FirstName = "first", LastName = "last", Password = "pass", Username = "user" });
                context.SaveChanges();
            }
            int ans = -1;
            using (var context = new MyDbContext(options))
            {
                ans = BusinessLogic.GetCustId("user", context);
            }
            Assert.Equal(1, ans);
        }
        [Fact]
        public void TestUsernameSearch()
        {
            bool ans = false;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestUsernameSearch").Options;
            using (var context = new MyDbContext(options))
            {
                Customer newCust = new Customer { CustomerId = 1, FirstName = "first", LastName = "last", Password = "pass", Username = "user" };
                context.Customers.Add(newCust);
                context.SaveChanges();
                ans = BusinessLogic.SearchUsername("user", context);
            }

            Assert.True(ans);

        }
        [Fact]
        public void TestGetLocationList()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestGetCartTotal").Options;
            using (var context = new MyDbContext(options))
            {
                var stores = new List<Store>
                {
                new Store{ Location = "Irving"},
                new Store{ Location = "Plano"},
                new Store{ Location = "Arlington"},
                new Store{ Location = "Lewisville"},
                new Store{ Location = "Mesquite"},
                };
                context.AddRange(stores);
                context.SaveChanges();

                var numList = new List<int>();

                numList.Add(1);
                numList.Add(2);
                numList.Add(3);
                numList.Add(4);
                numList.Add(5);

                List<string> anslist = BusinessLogic.GetLocationNameList(numList, context);
                Assert.Equal(stores[0].Location, anslist[0]);
            }
        }
        [Fact]
        public void TestRemoveFromCart()
        {

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            memoryCache.Set("currentCust", "cust");
            var numList = new List<int>();

            numList.Add(1);
            numList.Add(2);
            numList.Add(3);
            numList.Add(4);
            numList.Add(5);

            var doubleList = new List<double>();

            doubleList.Add(1);
            doubleList.Add(2);
            doubleList.Add(3);
            doubleList.Add(4);
            doubleList.Add(5);

            List<string> sList = new List<string>();

            sList.Add(" ");
            sList.Add(" ");
            sList.Add(" ");
            sList.Add(" ");
            sList.Add(" ");

            memoryCache.Set("prodIds", numList);
            memoryCache.Set("locIds", numList);
            memoryCache.Set("totalCost", 100);
            memoryCache.Set("costs", doubleList);
            memoryCache.Set("currentCust", "cust");
            memoryCache.Set("prodNames", sList);
            memoryCache.Set("quantities", numList);

            BusinessLogic.RemoveFromCart(1, memoryCache);
            List<int> firstProd = (List<int>)memoryCache.Get("prodIds");
            int first = firstProd[0];

            Assert.Equal(4, first);
        }
        [Fact]
        public void TestCheckInventory()
        {
            bool result = true;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestOrderAddingToDb").Options;
            using (var context = new MyDbContext(options))
            {
                var productList = new List<Product>()
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3 },
                };
                context.Products.AddRange(productList);
                context.SaveChanges();

                result = BusinessLogic.CheckInventory(51, 1, context);

                Assert.False(result);

            }
        }
        [Fact]
        public void TestControllerProductListSort()
        {
            int count = -1;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestControllerProductListSort").Options;
            using (var context = new MyDbContext(options))
            {
                var productList = new List<Product>()
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3 },
                };
                context.Products.AddRange(productList);
                context.SaveChanges();

                var onlyStoreProducts = context.Products.Where(x => x.StoreId == 1);
                count = onlyStoreProducts.Count();
            }
            Assert.Equal(1, count);

        }
        [Fact]
        public void TestEmptyCartCheck()
        {
            string result = "";
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            if (!memoryCache.TryGetValue("prodIds", out List<int> prodIds))
            {
                result = "ViewEmptyCart";
            }
            Assert.Equal("ViewEmptyCart", result);
        }
        [Fact]
        public void TestProductDuplicateCheck()
        {
            List<int> prodIds = new List<int>();
            List<int> quantities = new List<int>();

            prodIds.Add(1);
            prodIds.Add(2);


            quantities.Add(1);
            quantities.Add(4);

            int newProdid = 1;
            int newQuantity = 2;
            int duplicateIndex = -1;
            int result = -1;

            if (prodIds.Contains(newProdid))
            {
                duplicateIndex = prodIds.IndexOf(newProdid);
                quantities[duplicateIndex] += newQuantity;
                result = quantities[duplicateIndex];
            }

            Assert.Equal(3, result);
        }
        [Fact]
        public void TestOrderTotalByLocationSort()
        {
            double revanue = 0;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestOrderTotalByLocationSort").Options;

            using (var context = new MyDbContext(options))
            {

                var productList = new List<Product>()
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3 },
                };
                context.Products.AddRange(productList);
                context.SaveChanges();

                var stores = new List<Store>
            {
                new Store{ Location = "Irving"},
                new Store{ Location = "Plano"},
                new Store{ Location = "Arlington"},
                new Store{ Location = "Lewisville"},
                new Store{ Location = "Mesquite"},
            };
                context.AddRange(stores);
                context.SaveChanges();

                Cart newCart = BusinessLogic.NewCart();
                newCart.Costs.Add(100);
                newCart.Costs.Add(200);
                newCart.Costs.Add(300);
                newCart.CustId = 1;
                newCart.LocIds.Add(1);
                newCart.LocIds.Add(2);
                newCart.LocIds.Add(3);
                newCart.OrderQuantity.Add(3);
                newCart.OrderQuantity.Add(2);
                newCart.OrderQuantity.Add(1);
                newCart.ProdIds.Add(1);
                newCart.ProdIds.Add(2);
                newCart.ProdIds.Add(3);
                newCart.ProdNames.Add("a");
                newCart.ProdNames.Add("b");
                newCart.ProdNames.Add("c");
                newCart.TotalCost = 0.00;

                BusinessLogic.AddCartToDb(newCart, context);

                revanue = BusinessLogic.LocationRevanue(1, context);
            }
            Assert.Equal(300, revanue);
        }
        [Fact]
        public void TestUserOrderTotalSort()
        {
            double total = 0;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestUserOrderTotalSort").Options;

            using (var context = new MyDbContext(options))
            {

                var productList = new List<Product>()
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3 },
                };
                context.Products.AddRange(productList);
                context.SaveChanges();

                var stores = new List<Store>
            {
                new Store{ Location = "Irving"},
                new Store{ Location = "Plano"},
                new Store{ Location = "Arlington"},
                new Store{ Location = "Lewisville"},
                new Store{ Location = "Mesquite"},
            };
                context.AddRange(stores);
                context.SaveChanges();

                Cart newCart = BusinessLogic.NewCart();
                newCart.CustId = 1;
                newCart.Costs.Add(100);
                newCart.Costs.Add(200);
                newCart.Costs.Add(300);
                newCart.CustId = 1;
                newCart.LocIds.Add(1);
                newCart.LocIds.Add(2);
                newCart.LocIds.Add(3);
                newCart.OrderQuantity.Add(3);
                newCart.OrderQuantity.Add(2);
                newCart.OrderQuantity.Add(1);
                newCart.ProdIds.Add(1);
                newCart.ProdIds.Add(2);
                newCart.ProdIds.Add(3);
                newCart.ProdNames.Add("a");
                newCart.ProdNames.Add("b");
                newCart.ProdNames.Add("c");
                newCart.TotalCost = 0.00;

                BusinessLogic.AddCartToDb(newCart, context);

                total = BusinessLogic.UserOrderTotal(1, context);
            }
            Assert.Equal(1000, total);
        }
        [Fact]
        public void TestSubcartFormatCount()
        {
            int numOfOrderGroups = -1;
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(databaseName: "TestSubcartFormatCount").Options;

            using (var context = new MyDbContext(options))
            {

                var productList = new List<Product>()
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3 },
                };
                context.Products.AddRange(productList);
                context.SaveChanges();

                var stores = new List<Store>
            {
                new Store{ Location = "Irving"},
                new Store{ Location = "Plano"},
                new Store{ Location = "Arlington"},
                new Store{ Location = "Lewisville"},
                new Store{ Location = "Mesquite"},
            };
                context.AddRange(stores);
                context.SaveChanges();

                Cart newCart = BusinessLogic.NewCart();
                newCart.CustId = 1;
                newCart.Costs.Add(100);
                newCart.Costs.Add(200);
                newCart.Costs.Add(300);
                newCart.CustId = 1;
                newCart.LocIds.Add(1);
                newCart.LocIds.Add(2);
                newCart.LocIds.Add(3);
                newCart.OrderQuantity.Add(3);
                newCart.OrderQuantity.Add(2);
                newCart.OrderQuantity.Add(1);
                newCart.ProdIds.Add(1);
                newCart.ProdIds.Add(2);
                newCart.ProdIds.Add(3);
                newCart.ProdNames.Add("a");
                newCart.ProdNames.Add("b");
                newCart.ProdNames.Add("c");
                newCart.TotalCost = 0.00;

                BusinessLogic.AddCartToDb(newCart, context);
                BusinessLogic.AddCartToDb(newCart, context);

                List<HistorySubcart> historySubcart = BusinessLogic.FormatLocOrderHistory(1, context);
                numOfOrderGroups = historySubcart.Count();
            }
            Assert.Equal(2, numOfOrderGroups);
        }
    }
}
