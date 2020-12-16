using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.Models.Task;

namespace TimeTracker.DAL
{
    public class TaskHandler
    {
        private readonly ApplicationDbContext _context;

        public TaskHandler(ApplicationDbContext context)
        {
            this._context = context;
        }

        public List<GetDaysDataResponse> GetDaysData(QueryTasks queryTasks)
        {
            var result = new List<GetDaysDataResponse>();

            var taskDayQuery = this._context.TaskDay.Where(
                    x => x.User.Guid == queryTasks.OwnerGuid &&
                    queryTasks.StartDate.Date <= x.Date.Date &&
                    x.Date.Date <= queryTasks.EndDate.Date).AsNoTracking();

            var taskDayGuids = taskDayQuery.Select(x => x.Guid);

            foreach (var taskDay in taskDayQuery.ToList())
            {
                var tasks = this._context.Task.Where(x => taskDayGuids.Contains(x.TaskDay.Guid) && x.Date.Date == taskDay.Date.Date)
                    .OrderBy(x => x.DisplayOrder)
                    .Include(x => x.TaskSource)
                    .Include(x => x.TaskType)
                    .AsNoTracking()
                    .ToList();

                result.Add(new GetDaysDataResponse()
                {
                    Guid = taskDay.Guid,
                    Date = taskDay.Date,
                    IsLeave = taskDay.IsLeave,
                    FormData = tasks
                });
            }

            return result;
        }

        public void CreateTasks(List<Task> tasks, IEnumerable<TaskDay> taskDays)
        {
            foreach ((var task, var taskDay) in tasks.Zip(taskDays))
            {
                task.Guid = Guid.NewGuid();

                task.TaskSource = this._context.TaskSource.Where(x => x.Guid == task.TaskSource.Guid).SingleOrDefault();
                task.TaskType = this._context.TaskType.Where(x => x.Guid == task.TaskType.Guid).SingleOrDefault();
                task.TaskDay = taskDay;

                this._context.Task.Add(task);
            }

            _context.SaveChanges();
        }


        public TaskDay CreateTaskDay(DateTime date, Guid ownerGuid, bool isLeave = false)
        {
            var owner = this._context.User.Where(x => x.Guid == ownerGuid).SingleOrDefault();
            var taskDay = new TaskDay(date, owner)
            {
                IsLeave = isLeave
            };
            _context.TaskDay.Add(taskDay);
            _context.SaveChanges();
            return taskDay;
        }

        public void CreateOrUpdateTaskDay(DateTime date, Guid ownerGuid, bool isLeave)
        {
            var taskDay = this._context.TaskDay.Where(x => x.User.Guid == ownerGuid && x.Date.Date == date.Date).SingleOrDefault();

            if (taskDay == null)
            {
                this.CreateTaskDay(date, ownerGuid, isLeave);
            }
            else {
                taskDay.IsLeave = isLeave;
                taskDay.SetUpdatedDate();
                _context.SaveChanges();
            }
        }

        public IEnumerable<TaskDay> CreateAndSelectTaskDay(List<Task> tasks, Guid ownerGuid)
        {
            var taskDays = new List<TaskDay>();
            foreach (var task in tasks)
            {
                var taskDay = this._context.TaskDay.Where(x => 
                    x.Date.Date == task.Date.Date &&
                    x.User.Guid == ownerGuid).SingleOrDefault();
                if (taskDay == null)
                {
                    taskDay = CreateTaskDay(task.Date, ownerGuid);
                }
                taskDays.Add(taskDay);
            }

            return taskDays;
        }

        public void DeleteTasks(DeleteTasks deleteTasks)
        {
            var tasks = this._context.Task.Where(x => deleteTasks.TaskGuids.Contains(x.Guid));
            if (tasks == null)
            {
                return;
            }
            this._context.Task.RemoveRange(tasks);
            _context.SaveChanges();
        }

        public List<Task> UpdateTaskRowOrder(List<UpdateTaskRowOrder> updateTaskRowOrders)
        {
            var updatedRow = new List<Task>();

            foreach (var updateTaskRowOrder in updateTaskRowOrders)
            {
                var task = this._context.Task.Where(x => x.Guid == updateTaskRowOrder.Guid).SingleOrDefault();
                if (task == null)
                {
                    continue;
                }
                task.DisplayOrder = updateTaskRowOrder.DisplayOrder;
                task.SetUpdatedDate();
                updatedRow.Add( task );
            }

            if (updatedRow.Count > 0)
            {
                _context.SaveChanges();
            }

            return updatedRow;
        }

        public List<UpdateTaskCol> UpdateTaskCol(List<UpdateTaskCol> updateTaskCols)
        {
            var updatedTaskCols = new List<UpdateTaskCol>();
            foreach (var updateTaskCol in updateTaskCols)
            {
                var task = this._context.Task.Where(x => x.Guid == updateTaskCol.Guid).SingleOrDefault();
                if (updateTaskCol.UpdateEntity( this._context ))
                {
                    updatedTaskCols.Add(updateTaskCol);
                }
            }

            if (updatedTaskCols.Count > 0)
            {
                _context.SaveChanges();
            }

            return updatedTaskCols;
        }
    }
}
