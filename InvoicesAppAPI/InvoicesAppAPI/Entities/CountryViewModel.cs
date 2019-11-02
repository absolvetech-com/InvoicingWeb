using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class CountryViewModel
    {
        public long CountryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Timezone { get; set; }
        public string CountryCode { get; set; }
    }
}
