using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace P1.Models
{
    public class Subcart//used to create a model with no lists as properties to properly bind it
    {
        public int Id { get; set; }
        [DisplayName("Product Name")]
        public string prodName { get; set; }
        [DisplayName("Cost Per Item")]
        public double cost { get; set; }
        [DisplayName("Quantity In Cart")]
        public int quantity { get; set; }
        [DisplayName("Store Location")]
        public string location { get; set; }
    }
}
