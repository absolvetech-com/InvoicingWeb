using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

namespace InvoicesAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region " Fields & Constructor "

        private UserManager<ApplicationUser> _userManager;
        private IUserService _userService;
        private IBussinessService _bussinessService;
        private IWebHostEnvironment _hostingEnvironment;

        public UserController(UserManager<ApplicationUser> userManager, IUserService _service, IBussinessService bussinessService, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _userService = _service;
            _bussinessService = bussinessService;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion


        #region " GetUserById "
        [HttpGet]
        [Authorize]
        [Route("GetUserById")]
        public async Task<JsonResult> GetUserById()//(string Id)
        {
            try
            {
                //to get userid from access token
                string Id = User.Claims.First(c => c.Type == "UserID").Value;
                UserDetailsViewModel _userDetails = new UserDetailsViewModel();
                var user = await _userManager.FindByIdAsync(Id);
                var userstatus = user.UserStatus;
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var bussiness = new BussinessDetailViewModel();
                    if (roles[0] == Constants.isAdmin)
                    {
                        //get bussiness details of admin by id
                        bussiness = await _bussinessService.GetBussinessDetailsById(user.Id);
                    }
                    else
                    {
                        //get bussiness details of parent admin if role is subadmin
                        bussiness = await _bussinessService.GetBussinessDetailsById(user.ParentUserId);
                    }
                    _userDetails.Id = user.Id;
                    _userDetails.Name = user.Name;
                    _userDetails.Email = user.Email;
                    _userDetails.PhoneNumber = user.PhoneNumber;
                    _userDetails.ProfilePic = user.ProfilePic;
                    _userDetails.Language = user.Language;
                    _userDetails.UserType = roles.FirstOrDefault();
                    _userDetails.DeviceToken = user.DeviceToken;
                    _userDetails.DeviceType = user.DeviceType;
                    _userDetails.AccessToken = user.AccessToken; 
                    _userDetails.ParentUserId = user.ParentUserId;
                    _userDetails.UserStatus = user.UserStatus;
                    _userDetails.IsActive = user.IsActive;
                    _userDetails.Dob = user.Dob;
                    _userDetails.Gender = user.Gender;
                    _userDetails.CreatedDate = user.CreatedDate;
                    _userDetails.BussinessDetails = bussiness;
                    return new JsonResult(new { status = StatusCodes.Status200OK, success = true, message = "user profile"+ ResponseMessages.msgShownSuccess, userstatus, user_info = _userDetails });
                }
                return new JsonResult(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, userstatus = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }

        #endregion


        #region " Update User"
        [HttpPost]
        [Authorize]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateViewModel userUpdateModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //get userid from access token
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    { 
                        if(!string.IsNullOrWhiteSpace(userUpdateModel.Name))
                        {
                            user.Name = userUpdateModel.Name;
                        }
                        if (!string.IsNullOrWhiteSpace(userUpdateModel.PhoneNumber))
                        {
                            user.PhoneNumber = userUpdateModel.PhoneNumber;
                        }
                        if (!string.IsNullOrWhiteSpace(userUpdateModel.Language))
                        {
                            user.Language = userUpdateModel.Language;
                        }
                        if (!string.IsNullOrWhiteSpace(userUpdateModel.Dob))
                        {
                            user.Dob = DateTime.ParseExact(userUpdateModel.Dob, "dd/MM/yyyy", null);
                        }
                        if (!string.IsNullOrWhiteSpace(userUpdateModel.Gender))
                        {
                            user.Gender = userUpdateModel.Gender;
                        } 
                        user.UpdatedBy = userId;
                        user.UpdatedDate = DateTime.Now;
                        IdentityResult res = await _userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        { 
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user profile"+ ResponseMessages.msgUpdationSuccess, userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgUserBlockedOrDeleted, userstatus = false });
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


        #region " Update Bussiness / Update Bussiness Profile"
        [HttpPost]
        [Authorize(Roles = "superadmin, admin")]
        [Route("UpdateBussinessProfile")]
        public async Task<IActionResult> UpdateBussinessProfile(BussinessDetailViewModel _bussinessmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //get userid from access token
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    {
                        _bussinessmodel.IdentityId = userId;
                        bool result = await _bussinessService.UpdateBussinessProfile(_bussinessmodel);
                        if(result)
                        { 
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "bussiness profile"+ ResponseMessages.msgUpdationSuccess, userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = ResponseMessages.msgDbConnectionError, userstatus = false }); 
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgBlockOrInactiveUserNotPermitted, userstatus = false });
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


        #region " GetLinkedUserById / Sub User By ParentId"
        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("LinkedUsers")]
        public async Task<IActionResult> GetLinkedUserById() 
        {
            try
            {
                //to get userid from access token
                string Id = User.Claims.First(c => c.Type == "UserID").Value;  
                if (!string.IsNullOrEmpty(Id))
                {
                    var linkedusers = await _userService.GetLinkedUsers(Id);
                    if (linkedusers == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound+"linked user.", userstatus = false });
                    } 
                    return Ok(new { status = StatusCodes.Status200OK, success = true, message = "linked users"+ ResponseMessages.msgListFoundSuccess, userstatus = true, linkedusers });
                }
                return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = ResponseMessages.msgSessionExpired, userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }

        #endregion


        #region " Delete Linked User"
        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("DeleteLinkedUser")]
        public async Task<IActionResult> DeleteLinkedUser(CommonIdViewModel _model)
        {
            try
            {
                //to get userid from access token
                string Id = User.Claims.First(c => c.Type == "UserID").Value;
                if (!string.IsNullOrEmpty(_model._Id))
                {
                    var linkeduser = await _userManager.FindByIdAsync(_model._Id);
                    if(linkeduser == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, userstatus = false });
                    }
                    linkeduser.UserStatus = false;
                    linkeduser.IsDeleted = true;
                    linkeduser.DeletedDate = DateTime.Now;
                    linkeduser.DeletedBy = Id;
                    IdentityResult res = await _userManager.UpdateAsync(linkeduser);
                    if (res.Succeeded)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user"+ ResponseMessages.msgDeletionSuccess, userstatus = true });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
                }
                return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect, userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }
        #endregion



        #region " FOR MOBILE USER "

        #region " Show User Profile "
        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("ShowProfile")]
        public async Task<JsonResult> ShowUserProfile()
        {
            try
            {
                //to get userid from access token
                string Id = User.Claims.First(c => c.Type == "UserID").Value;
                UserProfileViewModel _userDetails = new UserProfileViewModel();
                var user = await _userManager.FindByIdAsync(Id);
                var userstatus = user.UserStatus;
                if (user != null)
                { 
                    var bussiness = new BussinessDetailViewModel();
                    if (User.IsInRole(Constants.isSubAdmin))
                    {
                        //get bussiness details of parent admin if role is subadmin
                        bussiness = await _bussinessService.GetBussinessDetailsById(user.ParentUserId);
                    }
                    else
                    {
                        //get bussiness details of admin by id
                        bussiness = await _bussinessService.GetBussinessDetailsById(user.Id);
                    }
                      
                    _userDetails.Name = user.Name;
                    _userDetails.Email = user.Email;
                    _userDetails.Phone_no = user.PhoneNumber;
                    _userDetails.Profile_pic = (!string.IsNullOrEmpty(user.ProfilePic))? user.ProfilePic :""; 
                    _userDetails.userstatus = user.UserStatus; 
                    _userDetails.Company_name = (!string.IsNullOrEmpty(bussiness.BussinessName)) ? bussiness.BussinessName : "";
                    _userDetails.Web_address = (!string.IsNullOrEmpty(bussiness.WebAddress)) ? bussiness.WebAddress : "";
                    _userDetails.Fax = (!string.IsNullOrEmpty(bussiness.Fax)) ? bussiness.Fax : "";
                    return new JsonResult(new { status = StatusCodes.Status200OK, success = true, message = "user profile"+ ResponseMessages.msgShownSuccess, userstatus, user_info = _userDetails });
                }
                return new JsonResult(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, userstatus = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }

        #endregion


        #region " Show User Address "
        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("ShowAddress")]
        public async Task<JsonResult> ShowUserAddress()
        {
            try
            {
                //to get userid from access token
                string Id = User.Claims.First(c => c.Type == "UserID").Value;
                UserAddressViewModel _userDetails = new UserAddressViewModel();
                var user = await _userManager.FindByIdAsync(Id);
                var userstatus = user.UserStatus;
                if (user != null)
                {
                    var bussiness = new BussinessDetailViewModel();
                    if (User.IsInRole(Constants.isSubAdmin))
                    {
                        //get bussiness details of parent admin if role is subadmin
                        bussiness = await _bussinessService.GetBussinessDetailsById(user.ParentUserId);
                    }
                    else
                    {
                        //get bussiness details of admin by id
                        bussiness = await _bussinessService.GetBussinessDetailsById(user.Id);
                    } 

                    _userDetails.Address1 =(!string.IsNullOrEmpty(bussiness.Address1))? bussiness.Address1 :"";
                    _userDetails.Address2 = (!string.IsNullOrEmpty(bussiness.Address2)) ? bussiness.Address2 : "";
                    _userDetails.CountryId = bussiness.CountryId;
                    _userDetails.CountryName = (!string.IsNullOrEmpty(bussiness.CountryName)) ? bussiness.CountryName : "";
                    _userDetails.StateId = bussiness.StateId;
                    _userDetails.StateName = (!string.IsNullOrEmpty(bussiness.StateName)) ? bussiness.StateName : "";
                    _userDetails.City = (!string.IsNullOrEmpty(bussiness.City)) ? bussiness.City : "";
                    _userDetails.Postalcode = (!string.IsNullOrEmpty(bussiness.Postalcode)) ? bussiness.Postalcode : "";
                    return new JsonResult(new { status = StatusCodes.Status200OK, success = true, message = "user address"+ ResponseMessages.msgShownSuccess, userstatus, user_info = _userDetails });
                }
                return new JsonResult(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgNotFound + "user address.", userstatus = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }

        #endregion


        #region " Update Profile"
        [HttpPost]
        [Authorize]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel userUpdateModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //get userid from access token
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    {
                        if (!string.IsNullOrWhiteSpace(userUpdateModel.Name))
                        {
                            user.Name = userUpdateModel.Name;
                        }
                        if (!string.IsNullOrWhiteSpace(userUpdateModel.Phone_no))
                        {
                            user.PhoneNumber = userUpdateModel.Phone_no;
                        } 
                        user.UpdatedBy = userId;
                        user.UpdatedDate = DateTime.Now;
                        IdentityResult res = await _userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        {
                            BussinessDetailViewModel _bussinessmodel = new BussinessDetailViewModel();
                            _bussinessmodel.IdentityId = userId;
                            _bussinessmodel.BussinessName = userUpdateModel.Company_name;
                            _bussinessmodel.BussinessPhone = userUpdateModel.Phone_no;
                            _bussinessmodel.Fax = userUpdateModel.Fax;
                            _bussinessmodel.WebAddress = userUpdateModel.Web_address;
                            bool result = await _bussinessService.UpdateBussinessProfile(_bussinessmodel);
                            if (result)
                            {
                                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "profile"+ ResponseMessages.msgUpdationSuccess, userstatus });
                            }
                            else
                                return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = ResponseMessages.msgDbConnectionError, userstatus = false });                      
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, userstatus = false });
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


        #region " Update Address"
        [HttpPost]
        [Authorize]
        [Route("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(UserAddressViewModel _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //get userid from access token
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    {
                        BussinessDetailViewModel _bussinessmodel = new BussinessDetailViewModel();
                        _bussinessmodel.IdentityId = userId;
                        _bussinessmodel.Address1 = _model.Address1;
                        _bussinessmodel.Address2 = _model.Address2;
                        _bussinessmodel.CountryId = _model.CountryId;
                        _bussinessmodel.StateId = _model.StateId;
                        _bussinessmodel.City = _model.City;
                        _bussinessmodel.Postalcode = _model.Postalcode;
                        bool result = await _bussinessService.UpdateBussinessProfile(_bussinessmodel);
                        if (result)
                        {
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "address"+ ResponseMessages.msgUpdationSuccess, userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = ResponseMessages.msgDbConnectionError, userstatus = false }); 
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, userstatus = false });
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


        #region " Update Bussiness Currency / SetCurrency"
        [HttpPost]
        [Authorize]
        [Route("SetCurrency")]
        public async Task<IActionResult> UpdateBussinessCurrency(SetCurrencyViewModel _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //get userid from access token
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    {
                        BussinessDetailViewModel _bussinessmodel = new BussinessDetailViewModel();
                        _bussinessmodel.IdentityId = userId;
                        _bussinessmodel.CurrencyId = _model.CurrencyId; 
                        bool result = await _bussinessService.UpdateBussinessProfile(_bussinessmodel);
                        if (result)
                        {
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "currency"+ ResponseMessages.msgUpdationSuccess, userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = ResponseMessages.msgDbConnectionError, userstatus = false });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, userstatus = false });
                }
                else
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = ResponseMessages.msgParametersNotCorrect, userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong+ ex.Message, userstatus = false });
            }
        }
        #endregion


        #region " Update User Profile Pic"
        [HttpPost]
        [Authorize]
        [Route("UpdateProfilePic")]
        public async Task<IActionResult> UpdateProfilePic([FromForm]FileUploaderViewModel _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //get userid from access token
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    {
                        var filename = ContentDispositionHeaderValue.Parse(_model.Image.ContentDisposition).FileName.Trim('"');
                        filename = CommonMethods.EnsureCorrectFilename(filename);
                        filename = CommonMethods.RenameFileName(filename);
                        using (FileStream fs = System.IO.File.Create(GetPathAndFilename(filename, Constants.userImagesContainer)))
                        { 
                            _model.Image.CopyTo(fs);
                            fs.Flush(); 
                        } 
                        user.ProfilePic = filename; 
                        user.UpdatedBy = userId;
                        user.UpdatedDate = DateTime.Now;
                        IdentityResult res = await _userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        {
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user profile picture"+ ResponseMessages.msgUpdationSuccess, userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, userstatus = false });
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


        #region " Delete Profile Pic"
        [HttpPost]
        [Authorize]
        [Route("DeleteProfilePic")]
        public async Task<IActionResult> DeleteProfilePic()
        {
            try
            {
                //get userid from access token
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);
                var userstatus = user.UserStatus;
                if (user != null && userstatus)
                { 
                    user.ProfilePic = null;
                    user.UpdatedBy = userId;
                    user.UpdatedDate = DateTime.Now;
                    IdentityResult res = await _userManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user profile picture"+ ResponseMessages.msgDeletionSuccess, userstatus });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
                }
                else
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, userstatus = false });

            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ResponseMessages.msgSomethingWentWrong + ex.Message, userstatus = false });
            }
        }
        #endregion

        #endregion


        #region " Private Methods "
        private string GetPathAndFilename(string filename, string foldername)
        {
            string path = _hostingEnvironment.WebRootPath + "//" + foldername + "//";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + filename;
        }
        #endregion
    }
}