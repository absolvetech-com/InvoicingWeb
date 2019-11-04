using InvoicesAppAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class FilterationViewModel
    {
        public int Page { get; set; } = 1;
        public int ItemsPerPage { get; set; } = Constants.itemsPerPage;
        public string SortBy { get; set; }
        public bool Reverse { get; set; } = false;
        public string Search { get; set; } = null;
        public string UserId { get; set; }
    }
}
