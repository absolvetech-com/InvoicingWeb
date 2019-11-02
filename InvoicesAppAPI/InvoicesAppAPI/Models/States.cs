using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Models
{
    public class States
    {
        [Key]
        public long StateId { get; set; }
        public string Name { get; set; }
        //Foreign key 
        public long CountryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey("CountryId")]
        public virtual Countries Country { get; set; } 
    }
}
