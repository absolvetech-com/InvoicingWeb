using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

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
        private IWebHostEnvironment _hostingEnvironment; 

        public AccountController(UserManager<ApplicationUser> userManager, 
            IOptions<ApplicationSettings> appSettings,
            IBussinessService service,
            IUserService userService,
            IEmailManager emailSender,
            IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager; 
            _appSettings = appSettings.Value; 
            _bussinessService = service;
            _userService = userService;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment; 
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
                        var msg= $"Hi {applicationUser.Name}, <br/><br/> Your confirmation code to access your account is {applicationUser.ConfirmationCode}. <br/><br/>Thanks";
                        await _emailSender.SendEmailAsync( email: applicationUser.Email, subject: "Confirm Email", htmlMessage: msg);

                        if (model.Role == Constants.isAdmin && result.Succeeded)
                        {
                            var bussiness = new BussinessDetail()
                            {
                                IdentityId = applicationUser.Id,
                                UniqueBussinessId = Guid.NewGuid().ToString(), 
                                BussinessEmail = applicationUser.Email,
                                BussinessPhone = applicationUser.PhoneNumber,
                                CurrencyId= Constants.baseCurrencyId,
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
                            //5 sub admin check later
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
                if (!userstatus)
                {
                    return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "user blocked or deleted. please contact to administrator", userstatus = false });
                }
                if (user != null && userstatus && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    //check email is confirmed
                    if (!_userManager.IsEmailConfirmedAsync(user).Result)
                    { 
                        return Ok(new { status = StatusCodes.Status200OK, success = false, message = "email not confirmed.", userstatus });
                    }
                    // update user with device type and device token
                    user.DeviceToken = model.DeviceToken;
                    user.DeviceType = model.DeviceType;
                    // change the security stamp only on correct username/password
                    await _userManager.UpdateSecurityStampAsync(user);
                    // Get role assigned to the user
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
                    List<Permissions> permissionList = new List<Permissions>(); 
                    UserInfo _userinfo = new UserInfo(); 
                    _userinfo.Id = user.Id;
                    _userinfo.Name = user.Name;
                    _userinfo.ProfilePic = (user.ProfilePic!=null && user.ProfilePic!= "") ? GetImageUrl(Constants.userImagesContainer,user.ProfilePic) :""; 
                    _userinfo.Email = user.Email;
                    _userinfo.Status = user.IsActive;
                    _userinfo.CurrencyId = (bussiness != null) ? bussiness.CurrencyId : 0;
                    _userinfo.CurrencySymbol = (bussiness!=null && bussiness.CurrencySymbol != null && bussiness.CurrencySymbol!="")? bussiness.CurrencySymbol: "";
                    _userinfo.Currency = (bussiness != null && bussiness.CurrencyCode != null && bussiness.CurrencyCode != "") ? bussiness.CurrencyCode : "";
                    _userinfo.UserType = roles.FirstOrDefault(); 
                    _userinfo.Permissions_List = permissionList;//send later
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
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel _model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(_model.Email);
                if (user != null)
                {
                    bool result = _userService.ConfirmEmail(_model.Email, _model.Code).Result;
                    var userstatus = user.UserStatus;
                    if (result)
                    {
                        var webRoot = _hostingEnvironment.WebRootPath;  
                        //Get TemplateFile located at wwwroot/Templates/EmailTemplates/Confirm_Account_Registration_Success.html  
                        var pathToFile = _hostingEnvironment.WebRootPath
                                + Path.DirectorySeparatorChar.ToString()
                                + Constants.mainTemplatesContainer
                                + Path.DirectorySeparatorChar.ToString()
                                + Constants.emailTemplatesContainer
                                + Path.DirectorySeparatorChar.ToString()
                                + Constants.email_template_Confirm_Account_Registration_Success;

                        var subject = Constants.subject_Confirm_Account_Registration_Success;
                        var name = user.Name;
                        var body = new BodyBuilder();  
                        using (StreamReader reader = System.IO.File.OpenText(pathToFile))
                        { 
                            body.HtmlBody = reader.ReadToEnd();
                        } 
                        string messageBody = body.HtmlBody;
                        messageBody = messageBody.Replace("{name}", name);
                        messageBody = messageBody.Replace("{subject}", subject);
                        await _emailSender.SendEmailAsync(email: user.Email, subject: subject, htmlMessage: messageBody);  
                        return Ok(new { status = StatusCodes.Status200OK, success = true, message = "email confirmed successfully.", userstatus });
                    }
                    else
                    {
                        return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "otp is invalid.", userstatus });
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
                    if (user != null)
                    {
                        var userstatus = user.UserStatus;
                        // If user has to activate his email to confirm his account, the use code listing below 
                        if (!_userManager.IsEmailConfirmedAsync(user).Result)
                        {
                            return Ok(new { status = StatusCodes.Status200OK, success = false, message = "email not confirmed.", userstatus });
                        }
                         
                        int code = CommonMethods.GenerateOTP();
                        user.EmailOtp = code;
                        IdentityResult res = await _userManager.UpdateAsync(user);
                        if(res.Succeeded)
                        {
                            //sent email here with code using await  
                            var msg = $"Hi {user.Name}, <br/><br/> Your one time password is {code} for reseting password on invoicing. <br/><br/> Thanks";
                            await _emailSender.SendEmailAsync(email: user.Email, subject: "Reset Password", htmlMessage: msg); 
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
                            return Ok(new { status = StatusCodes.Status200OK, success = false, message = "email not confirmed.", userstatus });
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
        [Authorize]
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

        #region " ResendEmail "

        [HttpPost]
        [AllowAnonymous]
        [Route("ResendEmail")]
        public async Task<IActionResult> ResendEmail(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.Email); 
                    if (user != null)
                    {
                        var userstatus = user.UserStatus;
                        if (!userstatus)
                        {
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "user blocked or deleted. please contact to administrator", userstatus = false });
                        }
                        if (user.EmailConfirmed)
                        {
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "email already confirmed", userstatus });
                        }
                        user.ConfirmationCode = CommonMethods.GenerateOTP().ToString();
                        IdentityResult res = await _userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        {
                            //sent email here with code using await   
                            var msg = $"Hi {user.Name}, <br/><br/> Your confirmation code to access your account is {user.ConfirmationCode}. <br/><br/>Thanks";
                            await _emailSender.SendEmailAsync(email: user.Email, subject: "Confirmation Email", htmlMessage: msg);
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "confirmation code sent on your email.", userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
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

        #region " Change Email "

        [HttpPost]
        [Authorize]
        [Route("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "parameters are not correct.", userstatus = false });
                } 
                var newUser = await _userManager.FindByNameAsync(model.NewEmail) ?? await _userManager.FindByEmailAsync(model.NewEmail); 
                if (newUser == null)
                {
                    //get userid from access token
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var userstatus = user.UserStatus;
                    if (user != null && userstatus)
                    {
                        // If user has to activate his email to confirm his account, the use code listing below 
                        if (!_userManager.IsEmailConfirmedAsync(user).Result)
                        {
                            return Ok(new { status = StatusCodes.Status200OK, success = false, message = "email not confirmed.", userstatus });
                        } 
                        user.Otp = CommonMethods.GenerateOTP();
                        user.Newemail = model.NewEmail;
                        user.EmailOtp = CommonMethods.GenerateOTP();
                        IdentityResult res = await _userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        {
                            //sent email on old email here with code using await  
                            var msg = $"Hi {user.Name}, <br/><br/> Your one time password is {user.Otp} for changing email on invoicing. <br/><br/> Thanks";
                            await _emailSender.SendEmailAsync(email: user.Email, subject: "Reset Email", htmlMessage: msg);
                            //sent email on new email here with code using await  
                            var newmsg = $"Hi {user.Name}, <br/><br/> Your confirmation code to access your account is {user.EmailOtp} on invoicing. <br/><br/> Thanks";
                            await _emailSender.SendEmailAsync(email: user.Newemail, subject: "Reset Email", htmlMessage: newmsg);
                            return Ok(new { status = StatusCodes.Status200OK, success = true, message = "Reset email OTP sent on your both emails.", userstatus });
                        }
                        else
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });
                    }
                    else
                        return Ok(new { status = StatusCodes.Status404NotFound, success = false, message = "sorry, we couldn't found any user associated with this email.", userstatus = false });
                }
                else
                    return Ok(new { status = StatusCodes.Status406NotAcceptable, success = false, message = "the email provided already in used. please try with other email", userstatus = false });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message, userstatus = false });
            }
        }

        #endregion

        #region " Reset Email "

        [HttpPost]
        [Authorize]
        [Route("ResetEmail")]
        public async Task<IActionResult> ResetEmail(ResetEmailViewModel model)
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
                        // If user has to activate his email to confirm his account, the use code listing below 
                        if (!_userManager.IsEmailConfirmedAsync(user).Result)
                        {
                            return Ok(new { status = StatusCodes.Status200OK, success = false, message = "email not confirmed.", userstatus });
                        }
                         
                        if (user.Otp == Convert.ToInt32(model.OldEmailOTP) && user.EmailOtp == Convert.ToInt32(model.NewEmailOTP))
                        {
                            user.UserName = user.Newemail;
                            user.NormalizedUserName = user.Newemail;
                            user.Email = user.Newemail;
                            user.NormalizedEmail = user.Newemail;
                            user.Otp = null;
                            user.EmailOtp = null;
                            user.EmailchangeConfirmed = true;
                            user.EmailchangeCounter = (user.EmailchangeCounter == null || user.EmailchangeCounter == 0) ? 1 : user.EmailchangeCounter + 1;
                            user.UpdatedBy = userId;
                            user.UpdatedDate = DateTime.Now;
                            IdentityResult res = await _userManager.UpdateAsync(user);
                            if (res.Succeeded)
                            {
                                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "email reset successfully. please login with new email.", userstatus });
                            }
                            else
                                return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = res.Errors.First().Code, userstatus = false });                            
                        }
                        else
                        {
                            return Ok(new { status = StatusCodes.Status400BadRequest, success = false, message = "either new or old email otp code is invalid.", userstatus = false }); ;
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

        [Route("Logout")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);
                 
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok(new { status = StatusCodes.Status200OK, success = true, message = "user logout successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { status = StatusCodes.Status500InternalServerError, success = false, message = "something went wrong." + ex.Message });
            }
        }

        #endregion

         
        #region " Private Methods "
        private string GetImageUrl(string foldername, string filename)
        {
            //string url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request); 
            string scheme = HttpContext.Request.Scheme.ToString();
            string host = HttpContext.Request.Host.ToString();
            string baseUrl = scheme + "://" + host;
            var path = baseUrl + "/" + foldername + "/" + filename;
            return path;
        }
        #endregion
    }
}