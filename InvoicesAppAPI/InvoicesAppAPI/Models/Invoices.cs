using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Models
{
    public class Invoices
    {
        [Key]
        public long InvoiceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InvoiceNumber { get; set; }
        public string PosoNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string ItemList { get; set; }
        public string Status { get; set; }
        public long CurrencyId { get; set; }
        public string Type { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; } 
        public decimal Total { get; set; }
        public long CustomerId { get; set; }
        public bool IsSent { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
        public string TermsAndConditions { get; set; }
        public DateTime? ConvertedDate { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currencies Currency { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customers Customer { get; set; }
    }
}
