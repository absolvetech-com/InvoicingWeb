using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Entities.Mobile;
using InvoicesAppAPI.Helpers;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Newtonsoft.Json;

namespace InvoicesAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        #region " Fields & Constructor "

        private UserManager<ApplicationUser> _userManager;
        private IInvoiceService _invoiceService;
        private IBussinessService _bussinessService;
        private readonly IEmailManager _emailSender;
        private IWebHostEnvironment _hostingEnvironment;

        public InvoiceController(UserManager<ApplicationUser> userManager, IInvoiceService invoiceService, IBussinessService service,
            IEmailManager emailSender, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _invoiceService = invoiceService;
            _bussinessService = service;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region " Create Invoice"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice(InvoiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //get user id from access token i.e authorized user
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    //if customer created by sub-admin then customer linked to parent admin (i.e bussiness)
                    string parentUserId = Id;
                    var user = await _userManager.FindByIdAsync(Id);
                    if (User.IsInRole(Constants.isSubAdmin))
                    {
                        parentUserId = user.ParentUserId;
                    }
                    var userstatus = user.UserStatus;
                    string invNo = string.Empty;
                    if(model.Type.ToUpper() == Constants.typeInvoice)
                    {
                        invNo = Constants.typeInvoice + CommonMethods.Generate15UniqueDigits();
                    }
                    else if (model.Type.ToUpper() == Constants.typeQuotation)
                    {
                        invNo = Constants.typeQuotation + CommonMethods.Generate15UniqueDigits();
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgInvoiceTypeInvalid, userstatus = false });
                    }

                    //for isSent field (false:- draft, true:- sent)
                    bool isSent = false; 
                    if(model.Status.ToUpper() == Constants.statusSent)
                    {
                        model.Status = Constants.statusUnpaidOrPending;
                        isSent = true;
                    }
                    else if (model.Status.ToUpper() == Constants.statusDrafted)
                    {
                        model.Status = Constants.statusDrafted;
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgInvoiceStatusInvalid, userstatus = false });
                    }

                    var invoices = new Invoices()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        InvoiceNumber = invNo,
                        PosoNumber = model.PosoNumber,
                        Date = DateTime.ParseExact(model.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(model.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ItemList = JsonConvert.SerializeObject(model.ItemList),
                        Status = model.Status,
                        CurrencyId = model.CurrencyId,
                        Type = model.Type.ToUpper(),
                        Subtotal = model.Subtotal,
                        Tax = model.Tax,
                        Total = model.Total,
                        CustomerId = model.CustomerId,
                        IsSent = isSent,
                        UserId = parentUserId,
                        Notes = model.Notes,
                        TermsAndConditions = model.TermsAndConditions,
                        IsPaid = false,
                        CreatedBy = Id,
                        CreatedDate = DateTime.Now
                    };
                    var retId = await _invoiceService.AddInvoice(invoices);
                    if (retId > 0)
                    {
                        if (invoices.IsSent)
                        {
                            await _invoiceService.SendInvoiceMail(retId);
                        }
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "invoice" + ResponseMessages.msgCreationSuccess, userstatus });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgDbConnectionError, userstatus = false });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
                }
            }
            return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect, userstatus = false });
        }
        #endregion


        #region " Update Invoice"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("UpdateInvoice")]
        public async Task<IActionResult> UpdateInvoice(InvoiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //get user id from access token i.e authorized user
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(Id);
                    var userstatus = user.UserStatus;
                    model.UserId = user.Id;
                    var retId = await _invoiceService.UpdateInvoice(model);
                    if (retId > 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "invoice"+ ResponseMessages.msgUpdationSuccess, userstatus });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgDbConnectionError, userstatus = false });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
                }
            }
            return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect, userstatus = false });
        }
        #endregion 


        #region " Get Invoice By Invoice Id"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("GetInvoice")]
        public async Task<IActionResult> GetInvoiceById(CommonNumericIdViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //to get userid from access token
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(Id);
                    var userstatus = user.UserStatus;
                    var invoice = await _invoiceService.GetInvoiceByInvoiceId(model._Id);
                    if (invoice == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "item.", userstatus = false });
                    }
                    return Ok(new { status = StatusCodes.Status200OK, success = true, message = "invoice"+ ResponseMessages.msgFoundSuccess, userstatus, invoice });
                }
                else
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect, userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
            }
        }

        #endregion


        #region " Get Invoice List"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("InvoiceList")]
        public async Task<IActionResult> GetInvoiceList(FilterationListViewModel model)
        {
            try
            {
                //to get userid from access token
                string UId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(UId);
                string parentUserId = user.Id;
                if (User.IsInRole(Constants.isSubAdmin))
                {
                    parentUserId = user.ParentUserId;
                }  
                var userstatus = user.UserStatus;
                var invoiceList = _invoiceService.GetInvoiceList(model, parentUserId);
                if (invoiceList == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "invoice.", userstatus = false });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "invoice"+ ResponseMessages.msgListFoundSuccess, userstatus, invoiceList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }

        #endregion


        #region " Delete Invoice"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("DeleteInvoice")]
        public async Task<IActionResult> DeleteInvoice(CommonNumericIdViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //to get userid from access token
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(Id);
                    var userstatus = user.UserStatus; 
                    var invoice = await _invoiceService.GetInvoiceByInvoiceId(model._Id);
                    if (invoice == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+"invoice.", userstatus = false });
                    }
                    invoice.UserId = Id;
                    bool res = await _invoiceService.DeleteInvoice(invoice);
                    if (res)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "invoice"+ ResponseMessages.msgDeletionSuccess, userstatus });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = ResponseMessages.msgDbConnectionError, userstatus = false });
                }
                else
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect, userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }
        #endregion

    }
}