using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;
using TimeTracker.Models.Period;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PeriodController : ControllerBase
    {
        protected readonly ApplicationDbContext _context;

        public PeriodController(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.User)]
        public IActionResult GetPeriods()
        {
            var result = this._context.Period.Select(x => x).AsNoTracking();
            return Ok(result);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult CreatePeriods(IEnumerable<Period> periods)
        {
            foreach (var period in periods)
            {
                period.Guid = Guid.NewGuid();
            }

            this._context.Period.AddRange(periods);
            this._context.SaveChanges();

            var result = periods.Select(x => x.Guid);
            return Ok(result);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult UpdatePeriods(IEnumerable<QueryPeriod> queryPeriods)
        {
            bool isUpdate = false;
            foreach (var queryPeriod in queryPeriods)
            {
                var period = this._context.Period.Where(x => x.Guid == queryPeriod.Guid).SingleOrDefault();

                if (period != null)
                {
                    period.StartDate = queryPeriod.StartDate;
                    period.EndDate = queryPeriod.EndDate;
                    period.Name= queryPeriod.Name;
                    period.SetUpdatedDate();
                    isUpdate = true;
                }
            }

            if (isUpdate)
            {
                this._context.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult DeletePeriods(IEnumerable<QueryPeriod> queryPeriods)
        {
            var targetGuids = queryPeriods.Select(x => x.Guid);
            var targetPeriods = this._context.Period.Where(x => targetGuids.Contains(x.Guid));

            if (targetPeriods != null)
            {
                this._context.Period.RemoveRange(targetPeriods);
                this._context.SaveChanges();
            }
            
            return Ok();
        }
    }
}
