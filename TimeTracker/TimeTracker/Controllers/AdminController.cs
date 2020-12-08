using System.Collections.Generic;
using System.Linq;
using ChatSample.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;
using TimeTracker.Helper.Extensions;
using TimeTracker.Hubs;
using TimeTracker.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(UserRoles.Admin)]
    public class AdminController : ControllerBase
    {
        protected readonly ApplicationDbContext _context;
        private readonly IHubContext<WSHub> _hubContext;
        private readonly WSHubHandler<WSHub> _hubMethods;

        public AdminController(ApplicationDbContext applicationDbContext, IHubContext<WSHub> hubContext, WSHubHandler<WSHub> hubMethods)
        {
            this._context = applicationDbContext;
            this._hubContext = hubContext;

            _hubMethods = hubMethods;
        }

        [HttpPost]
        public IActionResult GetUncheckAccounts()
        {
            var unCheckAccounts = this._context.User.Where(x => x.AccountStatus == AccountStatus.Uncheck).AsNoTracking();
            var returnData = new List<UserInfo>();
            foreach (var unCheckAccount in unCheckAccounts)
            {
                returnData.Add(new UserInfo(unCheckAccount));
            }
            return Ok(returnData);
        }

        [HttpPost]
        public IActionResult GetAccounts()
        {
            var accounts = this._context.User.Select(x => x).Include(x => x.UserRoles).AsNoTracking().ToList();
            var returnData = new List<UserInfo>();
            foreach (var account in accounts)
            {
                returnData.Add(new UserInfo(account));
            }
            return Ok(returnData);
        }

        [HttpPost]
        public IActionResult UpdateAccounts(IEnumerable<UpdateAccount> updateAccounts)
        {
            foreach (var updateAccount in updateAccounts)
            {
                var user = this._context.User.SingleOrDefault(x => x.Guid == updateAccount.Guid);
                if (user == null)
                {
                    continue;
                }

                if (updateAccount.IsUpdateName )
                {
                    user.Name = updateAccount.Name;
                    user.SetUpdatedDate();
                }

                if (updateAccount.IsUpdateAccountStatus)
                {
                    user.AccountStatus = updateAccount.AccountStatus;
                    user.SetUpdatedDate();
                }

                if (updateAccount.IsUpdateUserRoles)
                {
                    var model = _context.User
                        .Include(x => x.MapUserRoles)
                        .FirstOrDefault(x => x.Id == user.Id);

                    _context.TryUpdateManyToMany(model.MapUserRoles, updateAccount.UserRoles
                        .Select(x => new DAL.DBModels.MapUserRole
                        {
                            UserId = user.Id,
                            UserRolesId = x,
                        }), x => x.Id);
                }
            }
            _context.SaveChanges();

            foreach (var updateAccount in updateAccounts)
            {
                _hubMethods.ClientGetUserInfo(updateAccount.Guid);
            }
                
            return Ok();
        }
    }
}
