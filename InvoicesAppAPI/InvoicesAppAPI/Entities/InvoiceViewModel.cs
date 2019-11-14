using InvoicesAppAPI.Entities.Mobile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class InvoiceViewModel
    {
        public long InvoiceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        [Required]
        public string PosoNumber { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string DueDate { get; set; }
        [Required]
        public InvoiceItemsViewModel[] ItemList { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public long CurrencyId { get; set; }
        [Required]
        public string Type { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        [Required]
        public long CustomerId { get; set; } 
        public string UserId { get; set; }
        public string Notes { get; set; }
        public string TermsAndConditions { get; set; }
    }
}
