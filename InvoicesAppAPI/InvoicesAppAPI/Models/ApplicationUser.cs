using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string ConfirmationCode { get; set; }
        public string ProfilePic { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string AccessToken { get; set; }
        public string UserType { get; set; } 
        public int? Otp { get; set; } 
        public string ParentUserId { get; set; }
        public string Newemail { get; set; }
        public int? EmailOtp { get; set; }
        public bool? EmailchangeConfirmed { get; set; }
        public int? EmailchangeCounter { get; set; }
        public bool UserStatus { get; set; } //i.e active or block i.e deleted 
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
