using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Models
{
    public class Customers
    {
        [Key]
        public long CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BussinessName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string BillingAddress { get; set; }
        public string MailingAddress { get; set; }
        //Foreign key 
        public long? CountryId { get; set; }
        //Foreign key 
        public long? StateId { get; set; }
        public string City { get; set; }
        public string Postalcode { get; set; }
        public string PersonalEmail { get; set; }
        public string BussinessEmail { get; set; }
        public string Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string Gstin { get; set; }
        public long AccountNumber { get; set; }
        public string PosoNumber { get; set; }
        public string Website { get; set; }
        public string UserId { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        [ForeignKey("CountryId")]
        public virtual Countries Country { get; set; }
        [ForeignKey("StateId")]
        public virtual States State { get; set; }
    }
}
