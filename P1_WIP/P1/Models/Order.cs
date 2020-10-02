using System;
using System.Collections.Generic;
using System.Text;

namespace P1.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public double Price { get; set; }
        public int ProductId { get; set; }
        public DateTime CheckoutTime { get; set; }
        public int CartId { get; set; }
    }
}
