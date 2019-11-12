﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Entities.Models
{
    public class ResponseModel<T>
    { 
        public int totalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public string previousPage { get; set; }
        public string nextPage { get; set; }
        public string searchQuery { get; set; }
        public List<T> dataList { get; set; }
    }
}
