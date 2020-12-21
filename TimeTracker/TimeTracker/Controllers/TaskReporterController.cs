using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TimeTracker.DAL;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;
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
        public IActionResult GetSimpleSummary(QueryPeopleTasks queryPeopleTasks)
        {
            return Ok(this._taskReportHandler.GetSimpleSummary(queryPeopleTasks));
        }

        [HttpPost]
        public IActionResult GetTaskTimeSummaryDetail(QueryPeopleTasks queryPeopleTasks)
        {
            return Ok(this._taskReportHandler.GetTaskTimeSummaryDetail(queryPeopleTasks));
        }        

        [HttpPost]
        public IActionResult GetTaskTypeSummary(QueryPeopleTasks queryPeopleTasks)
        {
            return Ok(this._taskReportHandler.GetTaskTypeSummary(queryPeopleTasks));
        }

        [HttpPost]
        public IActionResult GetTaskSourceSummary(QueryPeopleTasks queryPeopleTasks)
        {
            return Ok(this._taskReportHandler.GetTaskSourceSummary(queryPeopleTasks));
        }

        [HttpPost]
        public IActionResult GetTaskTimeSummary(QueryPeopleTasks queryPeopleTasks)
        {
            return Ok(this._taskReportHandler.GetTaskTimeSummary(queryPeopleTasks));
        }
    }
}
