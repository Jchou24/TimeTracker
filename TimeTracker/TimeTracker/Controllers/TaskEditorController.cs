using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;
using TimeTracker.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(UserRoles.User)]
    public class TaskEditorController : ControllerBase
    {
        protected readonly ApplicationDbContext _context;

        public TaskEditorController(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }


        [HttpPost]
        public IActionResult GetAccounts()
        {
            var accounts = this._context.User.Select(x => x).AsNoTracking().ToList();
            var returnData = new List<ApprovedUser>();
            foreach (var account in accounts)
            {
                returnData.Add(new ApprovedUser(account));
            }
            return Ok(returnData);
        }
    }
}
