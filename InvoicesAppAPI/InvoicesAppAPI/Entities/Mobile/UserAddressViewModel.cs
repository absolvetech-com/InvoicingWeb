using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Mobile
{
    public class UserAddressViewModel
    {
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public long CountryId { get; set; }
        [Required]
        public long StateId { get; set; }
        [Required]
        public string City { get; set; }
        public string Postalcode { get; set; }
    }
}
