using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePic { get; set; }
        public string Language { get; set; } 
        public string UserType { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; } 
        public string AccessToken { get; set; }  
        public string Signature { get; set; }
        public string ParentUserId { get; set; } 
        public bool UserStatus { get; set; }  
        public bool IsActive { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public BussinessDetailViewModel BussinessDetails { get; set; }
    }
}
