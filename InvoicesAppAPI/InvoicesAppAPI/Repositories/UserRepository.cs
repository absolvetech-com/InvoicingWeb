using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Repositories
{
    public class UserRepository:IUserService
    {
        ApplicationDbContext db;
        public UserRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<bool> ConfirmEmail(string email, string code)
        {
            if (db != null)
            {
                ApplicationUser appuser = new ApplicationUser();
                appuser = db.Users.Where(s => s.Email == email && s.ConfirmationCode == code).FirstOrDefault();
                if (appuser != null)
                {
                    //update fields 
                    appuser.ConfirmationCode = "";
                    appuser.EmailConfirmed = true; 
                    db.Users.Update(appuser);
                    //Commit the transaction
                    await db.SaveChangesAsync();
                    return true;
                } 
                 return false;
            }
            return false;
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            if (db != null)
            {
                ApplicationUser appuser = new ApplicationUser();
                appuser = db.Users.Where(s => s.Email == model.Email && s.EmailOtp == Convert.ToInt32(model.Code)).FirstOrDefault();
                if (appuser != null)
                { 
                    appuser.EmailOtp = null;
                    appuser.PasswordHash = model.Password;
                    db.Users.Update(appuser); 
                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<List<LinkedUserViewModel>> GetLinkedUsers(string parentuserId)
        {
            if (db != null)
            {
                return await (from u in db.Users
                              where u.ParentUserId == parentuserId && u.UserStatus == true 
                              && (u.IsDeleted==false || u.IsDeleted==null)
                              select new LinkedUserViewModel
                              {
                                  Id = u.Id,
                                  Name= u.Name,
                                  Email=u.Email,
                                  PhoneNumber=u.PhoneNumber,
                                  ProfilePic=(u.ProfilePic!= null)? u.ProfilePic : "",
                                  Language=(u.Language != null) ? u.Language : "",
                                  UserStatus=u.UserStatus,
                                  IsActive=u.IsActive,
                                  Dob=(u.Dob != null)?u.Dob.Value.ToString("dd/MM/yyyy"):"",
                                  Gender=(u.Gender!=null)?u.Gender:"", 
                                  CreatedDate=(u.CreatedDate != null)? u.CreatedDate.Value.ToString("dd/MM/yyyy"):"" 
                              }).ToListAsync();
            } 
            return null;
        }
    }
}
