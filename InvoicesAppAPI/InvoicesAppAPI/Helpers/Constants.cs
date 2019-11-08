using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Helpers
{
    public static class Constants
    {
        public static readonly string isSuperAdmin = "superadmin"; 
        public static readonly string isAdmin = "admin";
        public static readonly string isSubAdmin = "subadmin";

        public static readonly long baseCurrencyId = 1; 
        public static readonly string baseLanguage = "English";

        //for filteration
        public static readonly int itemsPerPage = 30;
    }
}
