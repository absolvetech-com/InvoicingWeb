using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Entities.Mobile;
using InvoicesAppAPI.Entities.Models;
using InvoicesAppAPI.Helpers;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Repositories
{
    public class InvoiceRepository:IInvoiceService
    {
        ApplicationDbContext db;
        private IBussinessService _bussinessService;
        private readonly IEmailManager _emailSender;
        private IWebHostEnvironment _hostingEnvironment;
        public InvoiceRepository(ApplicationDbContext _db, IBussinessService service,
            IEmailManager emailSender, IWebHostEnvironment hostingEnvironment)
        {
            db = _db;
            _bussinessService = service;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
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
                obj.Status = model.Status;
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
                return await (from i in db.Invoices
                              join c in db.Currencies
                              on i.CurrencyId equals c.CurrencyId into cGroup
                              from c in cGroup.DefaultIfEmpty()
                              join cst in db.Customers
                              on i.CustomerId equals cst.CustomerId into cstGroup
                              from cst in cstGroup.DefaultIfEmpty()
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
                                  ItemList = JsonConvert.DeserializeObject<List<InvoiceItemsViewModel>>(i.ItemList),
                                  Status = i.Status,
                                  Type = i.Type,
                                  Subtotal = i.Subtotal,
                                  Tax = i.Tax,
                                  Total = i.Total,
                                  IsSent = i.IsSent,
                                  UserId = i.UserId,
                                  Notes = !string.IsNullOrWhiteSpace(i.Notes) ? i.Notes : "",
                                  TermsAndConditions = !string.IsNullOrWhiteSpace(i.TermsAndConditions) ? i.TermsAndConditions : "",
                                  ConvertedDate = (i.ConvertedDate != null) ? i.ConvertedDate.Value.ToString("dd/MM/yyyy") : "",
                                  IsPaid = i.IsPaid,
                                  PaymentDate = (i.PaymentDate != null) ? i.PaymentDate.Value.ToString("dd/MM/yyyy") : "",
                                  PaymentMode = (i.PaymentMode != null) ? i.PaymentMode : "",
                                  CurrencyId = i.CurrencyId,
                                  CurrencyName = c.Name,
                                  CurrencyCode=c.Code,
                                  CurrencySymbol=c.Symbol,
                                  CustomerId = i.CustomerId,
                                  FirstName=cst.FirstName,
                                  LastName=cst.LastName,
                                  BussinessName = cst.BussinessName,
                                  Phone = cst.Phone, 
                                  Mobile = cst.Mobile,   
                                  Address1 = cst.Address1,
                                  PersonalEmail = cst.PersonalEmail,
                                  BussinessEmail = !string.IsNullOrWhiteSpace(cst.BussinessEmail) ? cst.BussinessEmail : "", 
                                  Gstin = !string.IsNullOrWhiteSpace(cst.Gstin) ? cst.Gstin : "", 
                                  AccountNumber = cst.AccountNumber.ToString(), 
                                  Website = !string.IsNullOrWhiteSpace(cst.Website) ? cst.Website : ""
                              }).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<bool> DeleteInvoice(InvoiceListViewModel model)
        {
            if (db != null)
            {
                Invoices obj = new Invoices();
                obj = db.Invoices.Where(x => x.InvoiceId == model.InvoiceId).FirstOrDefault();
                obj.IsDeleted = true;
                obj.DeletedBy = model.UserId;
                obj.DeletedDate = DateTime.Now;
                db.Invoices.Update(obj);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public ResponseModel<InvoiceListViewModel> GetInvoiceList(FilterationListViewModel model, string UserId)
        {
            if (db != null)
            {
                //Return Lists 
                var source = (from i in db.Invoices
                              join c in db.Currencies
                              on i.CurrencyId equals c.CurrencyId into cGroup
                              from c in cGroup.DefaultIfEmpty()
                              join cst in db.Customers
                              on i.CustomerId equals cst.CustomerId into cstGroup
                              from cst in cstGroup.DefaultIfEmpty()
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
                                  ItemList = JsonConvert.DeserializeObject<List<InvoiceItemsViewModel>>(i.ItemList),
                                  Status = i.Status,
                                  Type = i.Type,
                                  Subtotal = i.Subtotal,
                                  Tax = i.Tax,
                                  Total = i.Total,
                                  IsSent = i.IsSent,
                                  UserId = i.UserId,
                                  Notes = !string.IsNullOrWhiteSpace(i.Notes) ? i.Notes : "",
                                  TermsAndConditions = !string.IsNullOrWhiteSpace(i.TermsAndConditions) ? i.TermsAndConditions : "",
                                  ConvertedDate = (i.ConvertedDate != null) ? i.ConvertedDate.Value.ToString("dd/MM/yyyy") : "",
                                  IsPaid = i.IsPaid,
                                  PaymentDate = (i.PaymentDate != null) ? i.PaymentDate.Value.ToString("dd/MM/yyyy") : "",
                                  PaymentMode = (i.PaymentMode != null) ? i.PaymentMode : "",
                                  CurrencyId = i.CurrencyId, 
                                  CurrencyName = c.Name,
                                  CurrencyCode = c.Code,
                                  CurrencySymbol = c.Symbol,
                                  CustomerId = i.CustomerId,
                                  FirstName = cst.FirstName,
                                  LastName = cst.LastName,
                                  BussinessName = cst.BussinessName,
                                  Phone = cst.Phone,
                                  Mobile = cst.Mobile,
                                  Address1 = cst.Address1,
                                  PersonalEmail = cst.PersonalEmail,
                                  BussinessEmail = !string.IsNullOrWhiteSpace(cst.BussinessEmail) ? cst.BussinessEmail : "",
                                  Gstin = !string.IsNullOrWhiteSpace(cst.Gstin) ? cst.Gstin : "",
                                  AccountNumber = cst.AccountNumber.ToString(),
                                  Website = !string.IsNullOrWhiteSpace(cst.Website) ? cst.Website : ""
                              }).AsQueryable();

                //Filter Parameter With null checks
                if (!string.IsNullOrEmpty(model.filterBy))
                {
                    var filterBy = model.filterBy.ToLower();
                    source = source.Where(m => m.Type.ToLower().Contains(filterBy)
                           || m.Status.ToLower().Contains(filterBy));
                }

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
                                                x.CurrencyCode.ToString().Contains(search) ||
                                                x.CurrencySymbol.ToString().Contains(search) ||
                                                x.FirstName.ToString().Contains(search) ||
                                                x.LastName.ToString().Contains(search) ||
                                                x.BussinessName.ToString().Contains(search) ||
                                                x.Phone.ToString().Contains(search) ||
                                                x.Mobile.ToString().Contains(search) ||
                                                x.PersonalEmail.ToString().Contains(search) ||
                                                x.BussinessEmail.ToString().Contains(search) ||
                                                x.Gstin.ToString().Contains(search) ||
                                                x.AccountNumber.ToString().Contains(search) ||
                                                x.Address1.ToString().Contains(search)
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


        #region " Send Invoice Email Template" 
        public async Task SendInvoiceMail(long InvoiceId)
        {
            try
            {
                InvoiceListViewModel invoiceDetails = new InvoiceListViewModel();
                invoiceDetails = await GetInvoiceByInvoiceId(InvoiceId);
                BussinessDetailViewModel adminbussinessDetials = new BussinessDetailViewModel();
                if (invoiceDetails != null)
                {
                    adminbussinessDetials = await _bussinessService.GetBussinessDetailsById(invoiceDetails.UserId);
                }

                var pathToFile = _hostingEnvironment.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + Constants.mainTemplatesContainer
                        + Path.DirectorySeparatorChar.ToString()
                        + Constants.invoicesTemplatesContainer
                        + Path.DirectorySeparatorChar.ToString()
                        + Constants.invoice_template_Sample_Invoice_Template;

                var subject = string.Empty;
                if (invoiceDetails.Type == Constants.typeInvoice)
                {
                    subject = Constants.subject_SendInvoice_to_customer + " Invoice No: # " + invoiceDetails.InvoiceNumber;
                }
                else
                {
                    subject = Constants.subject_SendQuotation_to_customer + " Quotation No: # " + invoiceDetails.InvoiceNumber;
                }

                string customerName = invoiceDetails.FirstName + " " + invoiceDetails.LastName;
                StringBuilder sb = new StringBuilder();
                foreach (var item in invoiceDetails.ItemList)
                {
                    sb.Append("<tr class='item'>");
                    sb.AppendFormat("<td>{0}</td>", item.Name);
                    sb.AppendFormat("<td>{0}</td>", item.Quantity);
                    sb.AppendFormat("<td>{0}</td>", item.Tax);
                    sb.AppendFormat("<td>{0}</td>", item.Price);
                    sb.Append("</tr>");
                }
                string itemList = sb.ToString();
                var body = new BodyBuilder();
                using (StreamReader reader = System.IO.File.OpenText(pathToFile))
                {
                    body.HtmlBody = reader.ReadToEnd();
                }
                string messageBody = body.HtmlBody;
                messageBody = messageBody.Replace("{companylogoUrl}", adminbussinessDetials.BussinessLogo);
                messageBody = messageBody.Replace("{invoiceNumber}", invoiceDetails.InvoiceNumber);
                messageBody = messageBody.Replace("{invoiceDate}", invoiceDetails.Date);
                messageBody = messageBody.Replace("{dueDate}", invoiceDetails.DueDate);
                messageBody = messageBody.Replace("{bussinessName}", adminbussinessDetials.BussinessName);
                messageBody = messageBody.Replace("{bussinessAddress}", CommonMethods.SplitLine(adminbussinessDetials.Address1));
                messageBody = messageBody.Replace("{customerBussiness}", invoiceDetails.BussinessName);
                messageBody = messageBody.Replace("{customerEmail}", invoiceDetails.PersonalEmail);
                messageBody = messageBody.Replace("{itemList}", itemList);
                messageBody = messageBody.Replace("{subTotal}", invoiceDetails.Subtotal.ToString());
                messageBody = messageBody.Replace("{tax}", invoiceDetails.Tax.ToString());
                messageBody = messageBody.Replace("{total}", invoiceDetails.Total.ToString());
                messageBody = messageBody.Replace("{customerName}", customerName);
                messageBody = messageBody.Replace("{currencySymbol}", invoiceDetails.CurrencySymbol); 
                await _emailSender.SendEmailAsync(email: invoiceDetails.PersonalEmail, subject: subject, htmlMessage: messageBody);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
