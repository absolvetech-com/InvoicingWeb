using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvoicesAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        #region " Fields & Constructor "

        private UserManager<ApplicationUser> _userManager;
        private IManagementService _managementService;

        public InvoiceController(UserManager<ApplicationUser> userManager, IManagementService managementService)
        {
            _userManager = userManager;
            _managementService = managementService;
        }
        #endregion


    }
}