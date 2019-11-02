using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicesAppAPI.Entities;
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
    public class UserController : ControllerBase
    {
        #region " Fields & Constructor "

        private UserManager<ApplicationUser> _userManager;
        private IUserService _userService;
        private IBussinessService _bussinessService;

        public UserController(UserManager<ApplicationUser> userManager, IUserService _service, IBussinessService bussinessService)
        {
            _userManager = userManager;
            _userService = _service;
            _bussinessService = bussinessService;
        }
        #endregion


        #region " GetUserById / ShowProfile /ViewProfile "
        [HttpGet]
        [Authorize]
        [Route("ShowProfile")]
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
                    return new JsonResult(new { status = StatusCodes.Status200OK, success = true, message = "show user profile successfully.", userstatus, user_info = _userDetails });
                }
                return new JsonResult(new { status = StatusCodes.Status404NotFound, success = false, message = "could not found any user.", userstatus = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message, userstatus = false });
            }
        }

        #endregion


        #region " Update User / UpdateProfile"
        [HttpPost]
        [Authorize]
        [Route("UpdateProfile")]
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
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user profile updated successfully.", userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "sorry, we couldn't find an account with that email or user is blocked.", userstatus = false });
                }
                else
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "parameters are not correct.", userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message, userstatus = false });
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
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "bussiness profile updated successfully.", userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "some error occurs", userstatus = false }); 
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "sorry, block or inactive user can't update bussiness detials.", userstatus = false });
                }
                else
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "parameters are not correct.", userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message, userstatus = false });
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
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not find any linked user.", userstatus = false });
                    } 
                    return Ok(new { status = StatusCodes.Status200OK, success = true, message = "linked users getting successfully.", userstatus = true, linkedusers });
                }
                return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "invalid access token or your current session has expired. please login again to keep all your service working", userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message, userstatus = false });
            }
        }

        #endregion


        #region " Delete Linked User"
        [HttpPost]
        [Authorize(Roles = "subadmin, admin")]
        [Route("DeleteLinkedUser")]
        public async Task<IActionResult> DeleteLinkedUser(string linkedUserId)
        {
            try
            {
                //to get userid from access token
                string Id = User.Claims.First(c => c.Type == "UserID").Value;
                if (!string.IsNullOrEmpty(linkedUserId))
                {
                    var linkeduser = await _userManager.FindByIdAsync(linkedUserId);
                    if(linkeduser == null)
                    {
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not find any linked user.", userstatus = false });
                    }
                    linkeduser.UserStatus = false;
                    linkeduser.IsDeleted = true;
                    linkeduser.DeletedDate = DateTime.Now;
                    linkeduser.DeletedBy = Id;
                    IdentityResult res = await _userManager.UpdateAsync(linkeduser);
                    if (res.Succeeded)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user deleted successfully.", userstatus = true });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
                }
                return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "passed parameter is not correct", userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message, userstatus = false });
            }
        }
        #endregion
    }
}