using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class StateViewModel
    {
        public long StateId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public long CountryId { get; set; }
    }
}
