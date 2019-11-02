using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Entities.Models;
using InvoicesAppAPI.Helpers;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InvoicesAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region " Fields & Constructor "

        private UserManager<ApplicationUser> _userManager; 
        private readonly ApplicationSettings _appSettings;
        private IBussinessService _bussinessService;
        private IUserService _userService;
        private readonly IEmailManager _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,  
            IOptions<ApplicationSettings> appSettings,
            IBussinessService service,
            IUserService userService,
            IEmailManager emailSender)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value; 
            _bussinessService = service;
            _userService = userService;
            _emailSender = emailSender;
        }

        #endregion

        #region " Register "

        [HttpPost]
        [Route("Register")] 
        public async Task<IActionResult> Register(ApplicationUserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                DeviceToken= model.DeviceToken,
                DeviceType=model.DeviceType,
                PhoneNumber=model.Phone,
                Language= string.IsNullOrEmpty(model.Language)? Constants.baseLanguage : model.Language,
                ParentUserId=model.ParentUserId, 
                ConfirmationCode=CommonMethods.GenerateOTP().ToString(),
                UserStatus = true,
                IsActive= true,
                CreatedBy= string.IsNullOrEmpty(model.ParentUserId)? "": model.ParentUserId,
                CreatedDate = DateTime.Now
            };


            if (model.Role == Constants.isSuperAdmin || model.Role == Constants.isAdmin || model.Role == Constants.isSubAdmin)
            {
                try
                {
                    var result = await _userManager.CreateAsync(applicationUser, model.Password);
                    await _userManager.AddToRoleAsync(applicationUser, model.Role);
                    if(result.Succeeded)
                    {
                        //send email here
                        //await _emailSender.SendEmailAsync( email: applicationUser.Email, subject: "Confirm Email", message: applicationUser.ConfirmationCode);

                        if (model.Role == Constants.isAdmin && result.Succeeded)
                        {
                            var bussiness = new BussinessDetail()
                            {
                                IdentityId = applicationUser.Id,
                                UniqueBussinessId = Guid.NewGuid().ToString(),
                                BaseCurrencySymbol = Constants.baseCurrencySymbol,
                                BaseCurrencyName = Constants.baseCurrencyName,
                                BussinessEmail = applicationUser.Email,
                                BussinessPhone = applicationUser.PhoneNumber,
                                CreatedBy = applicationUser.Id,
                                CreatedDate = DateTime.Now
                            };
                            bool status = await _bussinessService.Create(bussiness); 
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user registered succesfully." });
                        }
                        else if (model.Role == Constants.isSuperAdmin && result.Succeeded)
                        { 
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user registered succesfully." });
                        }
                        else if (model.Role == Constants.isSubAdmin && result.Succeeded)
                        { 
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user registered succesfully." });
                        }
                        else
                        {
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "something went wrong." });
                        } 
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = result.Errors.First().Code });
                    } 
                }
                catch (Exception ex)
                { 
                    return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = ex.Message.ToString() });
                }
            }
            else
            {  
                return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "requested user type is not authorised" });
            } 
        }

        #endregion

        #region " Login "

        [HttpPost]
        [Route("Login")] 
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            { 
                if (!ModelState.IsValid)
                { 
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success= false, message = "parameters are not correct.", userstatus = false });
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not found any user associated with this email.", userstatus = false });
                }
                var userstatus = user.UserStatus;
                if (user != null && userstatus && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    //check email is confirmed
                    if (!_userManager.IsEmailConfirmedAsync(user).Result)
                    { 
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "email not confirmed.", userstatus = false });
                    }
                    // change the security stamp only on correct username/password
                    await _userManager.UpdateSecurityStampAsync(user);
                    //Get role assigned to the user
                    var roles = await _userManager.GetRolesAsync(user);
                    IdentityOptions _options = new IdentityOptions();

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,roles.FirstOrDefault())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                    };

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
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var accessToken = tokenHandler.WriteToken(securityToken);
                    UserInfo _userinfo = new UserInfo();
                    _userinfo.Id = user.Id;
                    _userinfo.Name = user.Name;
                    _userinfo.ProfilePic = (user.ProfilePic!=null)? user.ProfilePic :""; 
                    _userinfo.Email = user.Email;
                    _userinfo.Status = user.IsActive; 
                    _userinfo.CurrencySymbol = (bussiness!=null && bussiness.BaseCurrencySymbol!="")? bussiness.BaseCurrencySymbol:"";
                    _userinfo.Currency = (bussiness != null && bussiness.BaseCurrencyName != "") ? bussiness.BaseCurrencyName : "";
                    _userinfo.UserType = roles.FirstOrDefault();
                    _userinfo.Permissions_List = null;//send later
                    _userinfo.AccessToken = accessToken;

                    var user_info = new Object();
                    {
                        user_info = _userinfo;
                    };
                    return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user successfully login.", userstatus, user_info });
                }
                else
                    return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "username or password is incorrect.", userstatus = false });
            }
            catch(Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong."+ ex.Message, userstatus = false });
            }
        }

        #endregion

        #region " ConfirmEmail "

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    bool result = _userService.ConfirmEmail(email, code).Result;
                    var userstatus = user.UserStatus;
                    if (result)
                    {
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "email confirmed successfully.", userstatus });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "otp is invalid.", userstatus=false });
                    }
                }
                else
                { 
                    return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "could not found any user associated with this email.", userstatus = false });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message, userstatus = false });
            }
        }

        #endregion

        #region " ForgotPassword "

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    {
                        // If user has to activate his email to confirm his account, the use code listing below 
                        if (!_userManager.IsEmailConfirmedAsync(user).Result)
                        {
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "email not confirmed.", userstatus = false });
                        }
                         
                        int code = CommonMethods.GenerateOTP();
                        user.EmailOtp = code;
                        IdentityResult res = await _userManager.UpdateAsync(user);
                        if(res.Succeeded)
                        {
                            //sent email here with code using await 
                            //await _userManager.SendEmailAsync(user.Id, "Reset Password", $"Please reset your password by using this {code}"); 
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "OTP sent on your email.", userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus=false });
                    }
                    else 
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "sorry, we couldn't found any user associated with this email.", userstatus = false });
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

        #region " ResetPassword "

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        { 
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    {
                        // If user has to activate his email to confirm his account, the use code listing below 
                        if (!_userManager.IsEmailConfirmedAsync(user).Result)
                        {
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "email not confirmed.", userstatus });
                        }

                        var newPassword = model.Password;
                        string hashedPassword = _userManager.PasswordHasher.HashPassword(user, newPassword);
                        model.Password = hashedPassword;
                        bool result = _userService.ResetPassword(model).Result;
                        if (result)
                        {
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "password reset successfully.", userstatus });
                        }
                        else
                        {
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "otp code is invalid.", userstatus = false }); ;
                        } 
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "sorry, we couldn't any user associated with this email.", userstatus });
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

        #region " ChangePassword "

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        { 
            try
            {
                if (ModelState.IsValid)
                {
                    //get userid from access token
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    //var user = await _userManager.FindByNameAsync(model.Email);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    { 
                        IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "password changed successfully.", userstatus });
                        }
                        else
                        {
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = result.Errors.First().Code, userstatus = false });
                        }
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "sorry, we couldn't any user associated with this email.", userstatus });
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

        #region " Logout "

        //[Route("Logout")]
        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return new ObjectResult("Success");
        //}

        #endregion
    }
}