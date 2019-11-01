using InvoicesAppAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Services
{
    public interface IUserService
    {
        Task<bool> ConfirmEmail(string email, string code);
        Task<bool> ResetPassword(ResetPasswordViewModel model);
        Task<List<LinkedUserViewModel>> GetLinkedUsers(string parentuserId);
    }
}
