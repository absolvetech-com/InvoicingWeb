using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Mobile
{
    public class CommonIdViewModel
    {
        [Required]
        public string _Id { get; set; }
    }
}
