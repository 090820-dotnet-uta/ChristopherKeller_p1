using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using P1.Data;
using P1;



namespace P1.Models
{
    static class Seeder
    {
        public static void SeedProducts(MyDbContext context)
        {
            var prod = new List<Product>
                {
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 1},
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 1},
                    new Product { Name = "Box Spring", Description = "This is a must for all king-sized beds!", Price = 89.99, Quantity = 50, StoreId = 1},
                    new Product { Name = "Pillow (Feather)", Description = "This is an extremely comfortable feather pillow!", Price = 24.99, Quantity = 50, StoreId = 1 },
                    new Product { Name = "Pillow (Microbead)", Description = "This is a modern form-fitting pillow!", Price = 22.99, Quantity = 50, StoreId = 1},
                    new Product { Name = "Pillow (Memory Foam)", Description = "This memory foam pillow will give you the ultimate support!", Price = 32.99, Quantity = 50, StoreId = 1},
                    new Product { Name = "Comforter", Description = "This comforter will fit a king size mattress.", Price = 28.99, Quantity = 50, StoreId = 1},
                    new Product { Name = "Sheets", Description = "This set of sheets will fit a king-sized bed.", Price = 42.99, Quantity = 50, StoreId = 1},
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 2},
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 2},
                    new Product { Name = "Box Spring", Description = "This is a must for all king-sized beds!", Price = 89.99, Quantity = 50, StoreId = 2},
                    new Product { Name = "Pillow (Feather)", Description = "This is an extremely comfortable feather pillow!", Price = 24.99, Quantity = 50, StoreId = 2 },
                    new Product { Name = "Pillow (Microbead)", Description = "This is a modern form-fitting pillow!", Price = 22.99, Quantity = 50, StoreId = 2},
                    new Product { Name = "Pillow (Memory Foam)", Description = "This memory foam pillow will give you the ultimate support!", Price = 32.99, Quantity = 50, StoreId = 2},
                    new Product { Name = "Comforter", Description = "This comforter will fit a king size mattress.", Price = 28.99, Quantity = 50, StoreId = 2},
                    new Product { Name = "Sheets", Description = "This set of sheets will fit a king-sized bed.", Price = 42.99, Quantity = 50, StoreId = 2},
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 3},
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 3 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 3},
                    new Product { Name = "Box Spring", Description = "This is a must for all king-sized beds!", Price = 89.99, Quantity = 50, StoreId = 3},
                    new Product { Name = "Pillow (Feather)", Description = "This is an extremely comfortable feather pillow!", Price = 24.99, Quantity = 50, StoreId = 3 },
                    new Product { Name = "Pillow (Microbead)", Description = "This is a modern form-fitting pillow!", Price = 22.99, Quantity = 50, StoreId = 3},
                    new Product { Name = "Pillow (Memory Foam)", Description = "This memory foam pillow will give you the ultimate support!", Price = 32.99, Quantity = 50, StoreId = 3},
                    new Product { Name = "Comforter", Description = "This comforter will fit a king size mattress.", Price = 28.99, Quantity = 50, StoreId = 3},
                    new Product { Name = "Sheets", Description = "This set of sheets will fit a king-sized bed.", Price = 42.99, Quantity = 50, StoreId = 3},
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 4},
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 4 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 4},
                    new Product { Name = "Box Spring", Description = "This is a must for all king-sized beds!", Price = 89.99, Quantity = 50, StoreId = 4},
                    new Product { Name = "Pillow (Feather)", Description = "This is an extremely comfortable feather pillow!", Price = 24.99, Quantity = 50, StoreId = 4 },
                    new Product { Name = "Pillow (Microbead)", Description = "This is a modern form-fitting pillow!", Price = 22.99, Quantity = 50, StoreId = 4},
                    new Product { Name = "Pillow (Memory Foam)", Description = "This memory foam pillow will give you the ultimate support!", Price = 32.99, Quantity = 50, StoreId = 4},
                    new Product { Name = "Comforter", Description = "This comforter will fit a king size mattress.", Price = 28.99, Quantity = 50, StoreId = 4},
                    new Product { Name = "Sheets", Description = "This set of sheets will fit a king-sized bed.", Price = 42.99, Quantity = 50, StoreId = 4},
                    new Product { Name = "Mattress Frame", Description = "This is a mattress frame to fit a king-sized mattress.", Price = 149.99, Quantity = 50, StoreId = 5},
                    new Product { Name = "Memory Foam Mattress", Description = "This is a king-sized memory foam mattress. The best of the best!", Price = 499.99, Quantity = 50, StoreId = 5 },
                    new Product { Name = "Spring Mattress", Description = "This is a king-sized spring mattress. Comfort at an affordable price!", Price = 274.99, Quantity = 50, StoreId = 5},
                    new Product { Name = "Box Spring", Description = "This is a must for all king-sized beds!", Price = 89.99, Quantity = 50, StoreId = 5},
                    new Product { Name = "Pillow (Feather)", Description = "This is an extremely comfortable feather pillow!", Price = 24.99, Quantity = 50, StoreId = 5 },
                    new Product { Name = "Pillow (Microbead)", Description = "This is a modern form-fitting pillow!", Price = 22.99, Quantity = 50, StoreId = 5},
                    new Product { Name = "Pillow (Memory Foam)", Description = "This memory foam pillow will give you the ultimate support!", Price = 32.99, Quantity = 50, StoreId = 5},
                    new Product { Name = "Comforter", Description = "This comforter will fit a king size mattress.", Price = 28.99, Quantity = 50, StoreId = 5},
                    new Product { Name = "Sheets", Description = "This set of sheets will fit a king-sized bed.", Price = 42.99, Quantity = 50, StoreId = 5},
                };
            context.AddRange(prod);
            context.SaveChanges();
        }

        public static void SeedStores(MyDbContext context)
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
        }
    }
}
