using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class CustomerListViewModel
    {
        public long CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BussinessName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Postalcode { get; set; }
        public string PersonalEmail { get; set; }
        public string BussinessEmail { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Gstin { get; set; }
        public string AccountNumber { get; set; }
        public string PosoNumber { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; }
    }
}
