using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class BussinessDetailViewModel
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public string UniqueBussinessId { get; set; }
        public string BussinessName { get; set; }
        public string BussinessLogo { get; set; }
        public string BussinessCoverPhoto { get; set; }
        public string AccountNumber { get; set; }
        public string BaseCurrencySymbol { get; set; }
        public string BaseCurrencyName { get; set; }
        public string CIN { get; set; }
        public string GSTIN { get; set; }
        public string BussinessSize { get; set; }
        public string BussinessClass { get; set; }
        public string Founded { get; set; }
        public string Fax { get; set; }
        public string WebAddress { get; set; }
        public string BussinessEmail { get; set; }
        public string BussinessPhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Postalcode { get; set; }
        public string Signature { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}
