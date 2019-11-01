using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Helpers
{
    public static class CommonMethods
    {
        public static int GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 99999); 
        }
    }
}
