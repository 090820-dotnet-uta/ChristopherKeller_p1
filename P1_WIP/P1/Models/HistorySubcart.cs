using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace P1.Models
{

    public class HistorySubcart
    {
        [DisplayName("Order")]
        public int cartId { get; set; }
        [DisplayName("Product Id")]
        public int prodId { get; set; }
        [DisplayName("Product")]
        public string prodName { get; set; }
        [DisplayName("Amount Ordered")]
        public int quantity { get; set; }
        [DisplayName("Cost per Product")]
        public double prodCost { get; set; }
        [DisplayName("Location Id")]
        public int locId { get; set; }
        [DisplayName("Checkout Time")]
        public DateTime time { get; set; }
    }
}
