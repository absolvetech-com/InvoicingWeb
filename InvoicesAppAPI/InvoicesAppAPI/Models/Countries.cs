using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Models
{
    public class Countries
    {
        [Key]
        public long CountryId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Timezone { get; set; }
        public string CountryCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsDeleted { get; set; } 
    }
}
