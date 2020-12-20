using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;
using TimeTracker.Models;
using TimeTracker.Models.Task;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(UserRoles.User)]
    public class TaskReporterController : ControllerBase
    {
        protected readonly ApplicationDbContext _context;
        protected readonly TaskReportHandler _taskReportHandler;

        public TaskReporterController(ApplicationDbContext applicationDbContext, TaskReportHandler taskReportHandler)
        {
            this._context = applicationDbContext;
            this._taskReportHandler = taskReportHandler;
        }

        [HttpPost]
        public IActionResult GetSimpleSummary(QueryTasks queryTasks)
        {
            return Ok(this._taskReportHandler.GetSimpleSummary(queryTasks));
        }

        [HttpPost]
        public IActionResult GetTaskTypeSummary(QueryTasks queryTasks)
        {
            return Ok(this._taskReportHandler.GetTaskTypeSummary(queryTasks));
        }

        [HttpPost]
        public IActionResult GetTaskSourceSummary(QueryTasks queryTasks)
        {
            return Ok(this._taskReportHandler.GetTaskSourceSummary(queryTasks));
        }

        [HttpPost]
        public IActionResult GetTaskTimeSummary(QueryTasks queryTasks)
        {
            return Ok(this._taskReportHandler.GetTaskTimeSummary(queryTasks));
        }
    }
}
