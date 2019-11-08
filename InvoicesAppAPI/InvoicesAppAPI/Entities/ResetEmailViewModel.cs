using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class ResetEmailViewModel
    {
        [Required]
        public string OldEmailOTP { get; set; }
        [Required]
        public string NewEmailOTP { get; set; }
    }
}
