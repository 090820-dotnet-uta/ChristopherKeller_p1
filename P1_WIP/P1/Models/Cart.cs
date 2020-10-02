using System;
using System.Collections.Generic;
using System.Text;
using P1.Models;

namespace P1.Models
{
    public class Cart
    {
        public List<int> LocIds { get; set; }
        public int CustId { get; set; }
        public List<int> ProdIds { get; set; }
        public List<double> Costs { get; set; }
        public List<int> OrderQuantity { get; set; }
        public double TotalCost { get; set; }
        public List<string> ProdNames { get; set; }
    }
}
