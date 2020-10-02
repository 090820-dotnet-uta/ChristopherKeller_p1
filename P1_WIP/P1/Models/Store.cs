using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace P1.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Location { get; set; }
    }
}
