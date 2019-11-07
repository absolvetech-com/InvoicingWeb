using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities
{
    public class FileUploaderViewModel
    {
        [Required]
        public IFormFile Image { get; set; } 
    }
}
