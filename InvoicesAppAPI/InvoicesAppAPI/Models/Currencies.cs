using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Models
{
    public class Currencies
    {
        [Key]
        public long CurrencyId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
