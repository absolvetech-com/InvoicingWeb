using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class CountryStateListViewModel
    {
        public long CountryId { get; set; } 
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Timezone { get; set; }
        public string CountryCode { get; set; }
        public List<StateViewModel> States { get; set; }
    }
}
