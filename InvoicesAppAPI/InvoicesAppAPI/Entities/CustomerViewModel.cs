using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class CustomerViewModel
    {
        public long CustomerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BussinessName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string BillingAddress { get; set; }
        public string MailingAddress { get; set; }
        //[Required]
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        //[Required]
        public long StateId { get; set; }
        public string StateName { get; set; }
        //[Required]
        public string City { get; set; }
        public string Postalcode { get; set; }
        [Required]
        [EmailAddress]
        public string PersonalEmail { get; set; }
        //[EmailAddress]
        public string BussinessEmail { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Gstin { get; set; }
        public string AccountNumber { get; set; }
        public string PosoNumber { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
    }
}
