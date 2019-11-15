using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Entities.Mobile;
using InvoicesAppAPI.Helpers;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvoicesAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        #region " Fields & Constructor "

        private UserManager<ApplicationUser> _userManager; 
        private IManagementService _managementService;

        public ManagementController(UserManager<ApplicationUser> userManager, IManagementService managementService)
        {
            _userManager = userManager;
            _managementService = managementService;
        }
        #endregion


        #region " Get Currencies List"
        [HttpGet]
        [Authorize]
        [Route("CurrencyList")]
        public async Task<IActionResult> GetCurrencyList() 
        {
            try
            {
                var currencyList = await _managementService.GetCurrencyList();
                if (currencyList == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "currency." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "currency"+ ResponseMessages.msgListFoundSuccess, currencyList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
            }
        }

        #endregion


        #region " Get Currency By Id"
        [HttpGet]
        [Authorize]
        [Route("GetCurrency")]
        public async Task<IActionResult> GetCurrencyById(long? currencyId)
        {
            try
            {
                if (currencyId == null)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect });
                } 
                var currency = await _managementService.GetCurrencyById(currencyId); 
                if (currency == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+"currency." });
                } 
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "currency"+ ResponseMessages.msgFoundSuccess, currency });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
            }
        }

        #endregion


        #region " Get Countries List"
        [HttpGet]
        [Authorize]
        [Route("CountriesList")]
        public async Task<IActionResult> GetCountryList()
        {
            try
            {
                var countriesList = await _managementService.GetCountryList();
                if (countriesList == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "country." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "country"+ ResponseMessages.msgListFoundSuccess, countriesList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
            }
        }

        #endregion


        #region " Get Country By Id"
        [HttpGet]
        [Authorize]
        [Route("GetCountry")]
        public async Task<IActionResult> GetCountryById(long? countryId)
        {
            try
            {
                if (countryId == null)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect });
                }
                var country = await _managementService.GetCountryById(countryId);
                if (country == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "country." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "country"+ ResponseMessages.msgFoundSuccess, country });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
            }
        }

        #endregion


        #region " Get Country State List"
        [HttpGet]
        [Authorize]
        [Route("GetCountryStateList")]
        public async Task<IActionResult> GetCountryStateList()
        {
            try
            {
                var countriesList = await _managementService.GetCountryStateList();
                if (countriesList == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "country." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "country"+ ResponseMessages.msgListFoundSuccess, countriesList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
            }
        }

        #endregion


        #region " Get State By Id"
        [HttpGet]
        [Authorize]
        [Route("GetStateById")]
        public async Task<IActionResult> GetStateById(long? stateId)
        {
            try
            {
                if (stateId == null)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect });
                }
                var state = await _managementService.GetStateById(stateId);
                if (state == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+ "state." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "state"+ ResponseMessages.msgFoundSuccess, state });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
            }
        }

        #endregion


        #region " Get State By CountryId"
        [HttpGet]
        [Authorize]
        [Route("StateListByCountry")]
        public async Task<IActionResult> GetStateByCountry(long? countryId)
        {
            try
            {
                if (countryId == null)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect });
                }
                var states = await _managementService.GetStateByCountry(countryId);
                if (states == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+ "state." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "state"+ ResponseMessages.msgListFoundSuccess, states });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
            }
        }

        #endregion


        #region " Add Currency"
        [HttpPost]
        [Authorize]
        [Route("AddCurrency")]
        public async Task<IActionResult> AddCurrency(CurrencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var currencies = new Currencies()
                    {
                        Name= model.Name,
                        Symbol=model.Symbol,
                        Code=model.Code,
                        CreatedBy=Id,
                        CreatedDate=DateTime.Now
                    };
                    var currencyId = await _managementService.AddCurrency(currencies);
                    if (currencyId > 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "currency"+ ResponseMessages.msgCreationSuccess });
                    }
                    else if(currencyId < 0)
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "currency"+ ResponseMessages.msgAlreadyExists });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgDbConnectionError });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
                } 
            }
            return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect });
        }
        #endregion


        #region " Add Country"
        [HttpPost]
        [Authorize]
        [Route("AddCountry")]
        public async Task<IActionResult> AddCountry(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var countries = new Countries()
                    {
                        Name = model.Name,
                        ShortName = model.ShortName,
                        Timezone = model.Timezone,
                        CountryCode = model.CountryCode,
                        CreatedBy = Id,
                        CreatedDate = DateTime.Now
                    };
                    var retId = await _managementService.AddCountry(countries);
                    if (retId > 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "country"+ ResponseMessages.msgCreationSuccess });
                    }
                    else if (retId < 0)
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "country"+ ResponseMessages.msgAlreadyExists });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgAlreadyExists });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
                }
            }
            return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect });
        }
        #endregion


        #region " Add State"
        [HttpPost]
        [Authorize]
        [Route("AddState")]
        public async Task<IActionResult> AddState(StateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var states = new States()
                    {
                        Name = model.Name, 
                        CountryId = model.CountryId,
                        CreatedBy = Id,
                        CreatedDate = DateTime.Now
                    };
                    var retId = await _managementService.AddState(states);
                    if (retId > 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "state"+ ResponseMessages.msgCreationSuccess });
                    }
                    else if (retId < 0)
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "state"+ ResponseMessages.msgAlreadyExists });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgDbConnectionError });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message });
                }
            }
            return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect });
        }
        #endregion

        //customer
        #region " Create Customer"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(CustomerViewModel model)
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
                    string accNo = "1000" + CommonMethods.Generate13UniqueDigits();
                    var customers = new Customers()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BussinessName = model.BussinessName,
                        Phone =model.Phone,
                        Fax=model.Fax,
                        Mobile=model.Mobile,
                        Address1=model.Address1,
                        Address2=model.Address2,
                        BillingAddress=model.BillingAddress,
                        MailingAddress=model.MailingAddress,
                        CountryId=model.CountryId,
                        StateId=model.StateId,
                        City=model.City,
                        Postalcode=model.Postalcode,
                        PersonalEmail=model.PersonalEmail,
                        BussinessEmail=model.BussinessEmail,
                        Gender=model.Gender,
                        Dob=(!string.IsNullOrWhiteSpace(model.Dob))? DateTime.ParseExact(model.Dob, "dd/MM/yyyy", null):(DateTime?)null,
                        Gstin=model.Gstin,
                        AccountNumber=Convert.ToInt64(accNo), //Convert.ToInt64(model.AccountNumber),
                        PosoNumber =model.PosoNumber,
                        Website=model.Website,
                        IsActive=true,
                        UserId=parentUserId,
                        CreatedBy = Id,
                        CreatedDate = DateTime.Now
                    };
                    var retId = await _managementService.AddCustomer(customers);
                    if (retId > 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "customer"+ ResponseMessages.msgCreationSuccess, userstatus });
                    }
                    else if (retId < 0)
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = ResponseMessages.msgEmailAlreadyUsed, userstatus=false });
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


        #region " Update Customer"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(CustomerViewModel model)
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
                    var retId = await _managementService.UpdateCustomer(model);
                    if (retId > 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "customer"+ ResponseMessages.msgUpdationSuccess, userstatus });
                    }
                    else if (retId < 0)
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = ResponseMessages.msgEmailAlreadyUsed, userstatus = false });
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


        #region " Get Customer By Id"
        [HttpPost]
        [Authorize]
        [Route("GetCustomer")]
        public async Task<IActionResult> GetCustomerById(CommonNumericIdViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //get user id from access token i.e authorized user
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(Id);
                    var userstatus = user.UserStatus;
                    var customer = await _managementService.GetCustomerById(model._Id);
                    if (customer == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "customer.", userstatus=false });
                    }
                    return Ok(new { status = StatusCodes.Status200OK, success = true, message = "customer"+ ResponseMessages.msgFoundSuccess, userstatus, customer });
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


        #region " Delete Customer"
        [HttpPost]
        [Authorize]
        [Route("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(CommonNumericIdViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //to get userid from access token
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(Id);
                    var userstatus = user.UserStatus;
                    long? customerId = Convert.ToInt32(model._Id); 
                    var customer = await _managementService.GetCustomerById(customerId);
                    if (customer == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "customer.", userstatus = false });
                    }
                    customer.UserId = Id;
                    bool res = await _managementService.DeleteCustomer(customer);
                    if (res)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "customer"+ ResponseMessages.msgDeletionSuccess, userstatus });
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


        #region " Get Customer List"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("CustomerList")]
        public async Task<IActionResult> GetCustomerList(FilterationListViewModel model)
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
                var customerList = _managementService.GetCustomerList(model, parentUserId);
                if (customerList == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+"customer.", userstatus=false });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "customer"+ ResponseMessages.msgListFoundSuccess, userstatus, customerList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }

        #endregion

        //item
        #region " Create Item"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("CreateItem")]
        public async Task<IActionResult> CreateItem(ItemViewModel model)
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
                    var items = new Items()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        Price = model.Price,
                        Tax = model.Tax, 
                        UserId = parentUserId,
                        CreatedBy = Id,
                        CreatedDate = DateTime.Now
                    };
                    var retId = await _managementService.AddItem(items);
                    if (retId > 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "item"+ ResponseMessages.msgCreationSuccess, userstatus });
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


        #region " Update Item"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("UpdateItem")]
        public async Task<IActionResult> UpdateItem(ItemViewModel model)
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
                    var retId = await _managementService.UpdateItem(model);
                    if (retId > 0)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "item"+ ResponseMessages.msgUpdationSuccess, userstatus });
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


        #region " Delete Item"
        [HttpPost]
        [Authorize]
        [Route("DeleteItem")]
        public async Task<IActionResult> DeleteItem(CommonNumericIdViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //to get userid from access token
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(Id);
                    var userstatus = user.UserStatus;
                    long? itemId = model._Id;
                    var item = await _managementService.GetItemById(itemId);
                    if (item == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+ "item.", userstatus = false });
                    }
                    item.UserId = Id;
                    bool res = await _managementService.DeleteItem(item);
                    if (res)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "item"+ ResponseMessages.msgDeletionSuccess, userstatus });
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


        #region " Get Item By Id"
        [HttpPost]
        [Authorize]
        [Route("GetItem")]
        public async Task<IActionResult> GetItem(CommonNumericIdViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //to get userid from access token
                    string Id = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(Id);
                    var userstatus = user.UserStatus;
                    var item = await _managementService.GetItemById(model._Id);
                    if (item == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+ "item.", userstatus=false });
                    }
                    return Ok(new { status = StatusCodes.Status200OK, success = true, message = "item"+ ResponseMessages.msgFoundSuccess, userstatus, item });
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


        #region " Get Item List"
        [HttpPost]
        [Authorize(Roles = "admin, subadmin")]
        [Route("ItemList")]
        public async Task<IActionResult> GetItemList(FilterationListViewModel model)
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
                var itemList = _managementService.GetItemList(model, parentUserId);
                if (itemList == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+"item.", userstatus = false });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "item"+ ResponseMessages.msgListFoundSuccess, userstatus, itemList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }

        #endregion
    }
}