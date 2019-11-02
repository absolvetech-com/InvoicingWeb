using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class CurrencyViewModel
    {
        public long CurrencyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string Code { get; set; }  
    }
}
