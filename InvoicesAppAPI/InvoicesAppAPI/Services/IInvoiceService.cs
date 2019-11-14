using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Entities.Models;
using InvoicesAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Services
{
    public interface IInvoiceService
    {
        Task<long> AddInvoice(Invoices model);

        Task<long> UpdateInvoice(InvoiceViewModel model);

        Task<InvoiceListViewModel> GetInvoiceByInvoiceId(long? Id);

        Task<bool> DeleteInvoice(InvoiceListViewModel model);

        ResponseModel<InvoiceListViewModel> GetInvoiceList(FilterationListViewModel model, string UserId);

        Task SendInvoiceMail(long InvoiceId);
    }
}
