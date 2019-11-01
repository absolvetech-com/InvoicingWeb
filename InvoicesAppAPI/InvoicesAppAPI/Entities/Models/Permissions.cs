using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Models
{
    public class Permissions
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool canView { get; set; }
        public bool canCreate { get; set; }
        public bool canUpdate { get; set; }
        public bool canDelete { get; set; }
    }
}
