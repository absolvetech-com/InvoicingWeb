using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class UserUpdateViewModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; } 
        public string Language { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; } 
    }
}
