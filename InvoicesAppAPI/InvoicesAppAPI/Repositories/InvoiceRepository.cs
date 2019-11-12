using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Entities.Models;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<long> AddInvoice(Invoices model)
        {
            if (db != null)
            {
                await db.Invoices.AddAsync(model);
                await db.SaveChangesAsync();
                return model.InvoiceId;
            }
            return 0;
        }

        public async Task<long> UpdateInvoice(InvoiceViewModel model)
        {
            if (db != null)
            {
                Invoices obj = new Invoices();
                obj = db.Invoices.Where(x => x.InvoiceId == model.InvoiceId).FirstOrDefault();
                obj.Title = model.Title;
                obj.Description = model.Description;
                obj.PosoNumber = model.PosoNumber;
                obj.Date = DateTime.ParseExact(model.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj.DueDate = DateTime.ParseExact(model.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj.ItemList = JsonConvert.SerializeObject(model.ItemList);
                obj.CurrencyId = model.CurrencyId;
                obj.Subtotal = model.Subtotal;
                obj.Tax = model.Tax;
                obj.Total = model.Total;
                obj.CustomerId = model.CustomerId;
                obj.Notes = model.Notes;
                obj.TermsAndConditions = model.TermsAndConditions;
                obj.UpdatedBy = model.UserId;
                obj.UpdatedDate = DateTime.Now;
                db.Invoices.Update(obj);
                await db.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public async Task<InvoiceListViewModel> GetInvoiceByInvoiceId(long? Id)
        {
            if (db != null)
            {
                List<ItemViewModel> objList = new List<ItemViewModel>(); 
                return await (from i in db.Invoices
                              where i.InvoiceId == Id
                              select new InvoiceListViewModel
                              {
                                  InvoiceId = i.InvoiceId,
                                  Title = i.Title,
                                  Description = i.Description,
                                  InvoiceNumber = i.InvoiceNumber,
                                  PosoNumber = i.PosoNumber,
                                  Date = (i.Date != null) ? i.Date.ToString("dd/MM/yyyy") : "",
                                  DueDate = (i.DueDate != null) ? i.DueDate.ToString("dd/MM/yyyy") : "",
                                  ItemList = JsonConvert.DeserializeObject<List<ItemViewModel>>(i.ItemList),
                                  Status = i.Status,
                                  Type = i.Type,
                                  Subtotal = i.Subtotal,
                                  Tax = i.Tax,
                                  Total = i.Total,
                                  IsSent = i.IsSent,
                                  UserId = i.UserId,
                                  Notes = i.Notes,
                                  TermsAndConditions = i.TermsAndConditions,
                                  ConvertedDate = (i.ConvertedDate != null) ? i.ConvertedDate.Value.ToString("dd/MM/yyyy") : "",
                                  IsPaid = i.IsPaid,
                                  PaymentDate = (i.PaymentDate != null) ? i.PaymentDate.Value.ToString("dd/MM/yyyy") : "",
                                  PaymentMode = (i.PaymentMode != null) ? i.PaymentMode : "",
                                  CurrencyId = i.CurrencyId,
                                  //CurrencyDetails = { },
                                  CustomerId = i.CustomerId,
                                  //CustomerDetails = { }
                              }).FirstOrDefaultAsync();
            }
            return null;
        }

        public ResponseModel<InvoiceListViewModel> GetInvoiceList(FilterationListViewModel model, string UserId)
        {
            if (db != null)
            {
                //Return Lists 
                var source = (from i in db.Invoices
                              where i.UserId == UserId && (i.IsDeleted == false || i.IsDeleted == null)
                              orderby i.InvoiceId descending
                              select new InvoiceListViewModel
                              {
                                  InvoiceId = i.InvoiceId,
                                  Title = i.Title,
                                  Description = i.Description,
                                  InvoiceNumber = i.InvoiceNumber,
                                  PosoNumber = i.PosoNumber,
                                  Date = (i.Date != null) ? i.Date.ToString("dd/MM/yyyy") : "",
                                  DueDate = (i.DueDate != null) ? i.DueDate.ToString("dd/MM/yyyy") : "",
                                  ItemList = JsonConvert.DeserializeObject<List<ItemViewModel>>(i.ItemList),
                                  Status = i.Status,
                                  Type = i.Type,
                                  Subtotal = i.Subtotal,
                                  Tax = i.Tax,
                                  Total = i.Total,
                                  IsSent = i.IsSent,
                                  UserId = i.UserId,
                                  Notes = i.Notes,
                                  TermsAndConditions = i.TermsAndConditions,
                                  ConvertedDate = (i.ConvertedDate != null) ? i.ConvertedDate.Value.ToString("dd/MM/yyyy") : "",
                                  IsPaid = i.IsPaid,
                                  PaymentDate = (i.PaymentDate != null) ? i.PaymentDate.Value.ToString("dd/MM/yyyy") : "",
                                  PaymentMode = (i.PaymentMode != null) ? i.PaymentMode : "",
                                  CurrencyId = i.CurrencyId,
                                  //CurrencyDetails = { },
                                  CustomerId = i.CustomerId,
                                  //CustomerDetails = { }
                              }).AsQueryable();

                //Search Parameter With null checks   
                if (!string.IsNullOrWhiteSpace(model.searchQuery))
                {
                    var search = model.searchQuery.ToLower();
                    source = source.Where(x =>
                                                x.Title.ToLower().Contains(search) ||
                                                x.Description.ToLower().Contains(search) ||
                                                x.InvoiceNumber.ToString().Contains(search) ||
                                                x.PosoNumber.ToString().Contains(search) ||
                                                x.Tax.ToString().Contains(search) ||
                                                x.Status.ToString().Contains(search)
                                                );
                }

                // Get's No of Rows Count   
                int count = source.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = model.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = model.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                // Returing List of Customers Collections  
                ResponseModel<InvoiceListViewModel> obj = new ResponseModel<InvoiceListViewModel>();
                obj.totalCount = TotalCount;
                obj.pageSize = PageSize;
                obj.currentPage = CurrentPage;
                obj.totalPages = TotalPages;
                obj.previousPage = previousPage;
                obj.nextPage = nextPage;
                obj.searchQuery = string.IsNullOrEmpty(model.searchQuery) ? "no parameter passed" : model.searchQuery;
                obj.dataList = items.ToList();
                return obj;
            }
            return null;
        }
    }
}
