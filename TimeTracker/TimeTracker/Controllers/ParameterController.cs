using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.DAL;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;

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

        // ======================================================================
        // TaskTypes
        [HttpPost]
        public IActionResult GetTaskTypes()
        {
            return Ok(this._parameterHandler.GetTaskTypes());
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult GetAllTaskTypes()
        {
            return Ok(this._parameterHandler.GetAllTaskTypes());
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult CreateTaskTypes(List<TaskType> taskTypes)
        {
            return Ok(this._parameterHandler.CreateTaskTypes(taskTypes));
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult UpdateTaskTypes(List<TaskType> taskTypes)
        {
            this._parameterHandler.UpdateTaskTypes(taskTypes);
            return Ok();
        }

        // ======================================================================
        // TaskSources
        [HttpPost]
        public IActionResult GetTaskSources()
        {
            return Ok(this._parameterHandler.GetTaskSources());
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult GetAllTaskSources()
        {
            return Ok(this._parameterHandler.GetAllTaskSources());
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult CreateTaskSources(List<TaskSource> taskSources)
        {
            return Ok(this._parameterHandler.CreateTaskSources(taskSources));
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Admin)]
        public IActionResult UpdateTaskSources(List<TaskSource> taskSources)
        {
            this._parameterHandler.UpdateTaskSources(taskSources);
            return Ok();
        }
    }
}
