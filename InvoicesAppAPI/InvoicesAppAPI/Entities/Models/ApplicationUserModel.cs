using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Models
{
    public class ApplicationUserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }  
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string Phone { get; set; }
        public string Language { get; set; }
        public string ParentUserId { get; set; }
    }
}
