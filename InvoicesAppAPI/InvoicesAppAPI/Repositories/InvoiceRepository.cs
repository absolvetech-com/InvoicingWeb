using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Repositories
{
    public class InvoiceRepository:IInvoiceService
    {
        ApplicationDbContext db;
        public InvoiceRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

    }
}
