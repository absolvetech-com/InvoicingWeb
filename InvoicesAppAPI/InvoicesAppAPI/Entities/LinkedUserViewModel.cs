using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class LinkedUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePic { get; set; }
        public string Language { get; set; } 
        public bool UserStatus { get; set; }
        public bool IsActive { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string CreatedDate { get; set; }
    }
}
