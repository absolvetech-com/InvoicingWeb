using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Models
{
    public class ResponseModel<T>
    {
        public int Count{ get; set; }
        public List<T> Data { get; set; }
    }
}
