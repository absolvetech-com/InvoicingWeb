using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProfilePic { get; set; }
        public string UserType { get; set; }
        public long CurrencyId { get; set; }
        public string CurrencySymbol { get; set; }
        public string Currency { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public bool Status { get; set; }
        public List<Permissions> Permissions_List { get; set; }
    }
}
