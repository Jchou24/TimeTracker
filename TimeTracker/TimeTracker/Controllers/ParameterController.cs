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
    public class ParameterController : ControllerBase
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ParameterHandler _parameterHandler;

        public ParameterController(ApplicationDbContext applicationDbContext, ParameterHandler parameterHandler)
        {
            this._context = applicationDbContext;
            this._parameterHandler = parameterHandler;
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.User)]
        public IActionResult GetDayWorkLimitTime()
        {
            return Ok(this._parameterHandler.GetLimitWorkTime());
        }

        [HttpPost]
        public IActionResult GetTaskTypes()
        {
            var result = this._context.TaskType.Where(x => x.IsActive == true).AsNoTracking().ToList();
            return Ok(result);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult GetAllTaskTypes()
        {
            var result = this._context.TaskType.Select(x => x).AsNoTracking().ToList();
            return Ok(result);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult CreateTaskTypes()
        {
            return Ok();
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult UpdateTaskTypes()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult GetTaskSources()
        {
            var result = this._context.TaskSource.Where(x => x.IsActive == true).AsNoTracking().ToList();
            return Ok(result);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult GetAllTaskSources()
        {
            var result = this._context.TaskSource.Select(x => x).AsNoTracking().ToList();
            return Ok(result);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult CreateTaskSources()
        {
            return Ok();
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult UpdateTaskSources()
        {
            return Ok();
        }

        /*
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
        */
    }
}
