using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Services
{
    public interface IBussinessService
    {
        Task<bool> Create(BussinessDetail bussinessDetail); 
        Task<BussinessDetailViewModel> GetBussinessDetailsById(string Id);
        Task<bool> UpdateBussinessProfile(BussinessDetailViewModel model);
    }
}
