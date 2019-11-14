using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Mobile
{
    public class InvoiceItemsViewModel
    { 
        public string Name { get; set; }
        public string Description { get; set; } 
        public int Quantity { get; set; } 
        public decimal Price { get; set; } 
        public decimal Tax { get; set; } 
    }
}
