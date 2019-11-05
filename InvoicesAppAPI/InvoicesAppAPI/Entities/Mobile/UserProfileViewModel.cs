using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Mobile
{
    public class UserProfileViewModel
    { 
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone_no { get; set; }
        [Required]
        public string Company_name { get; set; }
        public string Fax { get; set; }
        public string Web_address { get; set; }
        public string Profile_pic { get; set; }
        public bool userstatus { get; set; }
    }
}
