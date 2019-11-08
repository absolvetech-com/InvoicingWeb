using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Mobile
{
    public class CommonNumericIdViewModel
    {
        [Required]
        public long _Id { get; set; }
    }
}
