using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicesAppAPI.Entities;
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
        [AllowAnonymous]
        [Route("CurrencyList")]
        public async Task<IActionResult> GetCurrencyList() 
        {
            try
            {
                var currencyList = await _managementService.GetCurrencyList();
                if (currencyList == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not find any currency." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "currencies list successfully shown.", currencyList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
            }
        }

        #endregion


        #region " Get Currency By Id"
        [HttpGet]
        [AllowAnonymous]
        [Route("GetCurrency")]
        public async Task<IActionResult> GetCurrencyById(long? currencyId)
        {
            try
            {
                if (currencyId == null)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "passed parameter is not correct." });
                } 
                var currency = await _managementService.GetCurrencyById(currencyId); 
                if (currency == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not found currency." });
                } 
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "currency found successfully.", currency });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
            }
        }

        #endregion


        #region " Get Countries List"
        [HttpGet]
        [AllowAnonymous]
        [Route("CountriesList")]
        public async Task<IActionResult> GetCountryList()
        {
            try
            {
                var countriesList = await _managementService.GetCountryList();
                if (countriesList == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not find any country." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "country list successfully shown.", countriesList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
            }
        }

        #endregion


        #region " Get Country By Id"
        [HttpGet]
        [AllowAnonymous]
        [Route("GetCountry")]
        public async Task<IActionResult> GetCountryById(long? countryId)
        {
            try
            {
                if (countryId == null)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "passed parameter is not correct." });
                }
                var country = await _managementService.GetCountryById(countryId);
                if (country == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not found country." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "country found successfully.", country });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
            }
        }

        #endregion


        #region " Get State By Id"
        [HttpGet]
        [AllowAnonymous]
        [Route("GetStateById")]
        public async Task<IActionResult> GetStateById(long? stateId)
        {
            try
            {
                if (stateId == null)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "passed parameter is not correct." });
                }
                var state = await _managementService.GetStateById(stateId);
                if (state == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not found state." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "state found successfully.", state });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
            }
        }

        #endregion


        #region " Get State By CountryId"
        [HttpGet]
        [AllowAnonymous]
        [Route("StateListByCountry")]
        public async Task<IActionResult> GetStateByCountry(long? countryId)
        {
            try
            {
                if (countryId == null)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "passed parameter is not correct." });
                }
                var states = await _managementService.GetStateByCountry(countryId);
                if (states == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not found states." });
                }
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "states list found successfully.", states });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
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
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "currency created successfully." });
                    }
                    else if(currencyId < 0)
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "currency name already exists." });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "db connection error." });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
                } 
            }
            return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "parameters are not correct." });
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
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "country created successfully." });
                    }
                    else if (retId < 0)
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "country name already exists." });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "db connection error." });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
                }
            }
            return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "parameters are not correct." });
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
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "state created successfully." });
                    }
                    else if (retId < 0)
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "state name already exists." });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "db connection error." });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
                }
            }
            return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "parameters are not correct." });
        }
        #endregion
    }
}