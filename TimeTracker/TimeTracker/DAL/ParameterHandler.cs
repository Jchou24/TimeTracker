using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTracker.DAL.DBModels.Task;

namespace TimeTracker.DAL
{
    public class ParameterHandler
    {
        protected readonly ApplicationDbContext _context;

        public ParameterHandler(ApplicationDbContext context)
        {
            this._context = context;
        }

        public double GetLimitWorkTime()
        {
            return this._context.DayWorkLimitTime
                .OrderByDescending(x => x.CreatedDate)
                .Cacheable()
                .FirstOrDefault()
                .LimitWorkTime;
        }

        // ======================================================================
        // TaskTypes
        public IEnumerable<TaskType> GetTaskTypes()
        {
            return this._context.TaskType.Where(x => x.IsActive == true).AsNoTracking().ToList();
        }

        public IEnumerable<TaskType> GetAllTaskTypes()
        {
            return this._context.TaskType.Select(x => x).OrderByDescending(x => x.IsActive).ThenBy(x => x.DisplayName).AsNoTracking().ToList();
        }

        public IEnumerable<Guid> CreateTaskTypes(List<TaskType> taskTypes)
        {
            foreach (var taskType in taskTypes)
            {
                taskType.Guid = Guid.NewGuid();
            }

            this._context.TaskType.AddRange(taskTypes);
            this._context.SaveChanges();

            var result = taskTypes.Select(x => x.Guid);
            return result;
        }

        public void UpdateTaskTypes(List<TaskType> taskTypes)
        {
            bool isUpdate = false;
            foreach (var taskType in taskTypes)
            {
                var target = this._context.TaskType.Where(x => x.Guid == taskType.Guid).SingleOrDefault();

                if (target != null)
                {
                    target.CodeName = taskType.CodeName;
                    target.DisplayName = taskType.DisplayName;
                    target.IsActive = taskType.IsActive;
                    target.SetUpdatedDate();
                    isUpdate = true;
                }
            }

            if (isUpdate)
            {
                this._context.SaveChanges();
            }
        }
        // ======================================================================
        // TaskSources
        public IEnumerable<TaskSource> GetTaskSources()
        {
            return this._context.TaskSource.Where(x => x.IsActive == true).AsNoTracking().ToList();
        }

        public IEnumerable<TaskSource> GetAllTaskSources()
        {
            return this._context.TaskSource.Select(x => x).OrderByDescending(x => x.IsActive).ThenBy(x => x.DisplayName).AsNoTracking().ToList();
        }

        public IEnumerable<Guid> CreateTaskSources(List<TaskSource> taskSources)
        {
            foreach (var taskSource in taskSources)
            {
                taskSource.Guid = Guid.NewGuid();
            }

            this._context.TaskSource.AddRange(taskSources);
            this._context.SaveChanges();

            var result = taskSources.Select(x => x.Guid);
            return result;
        }

        public void UpdateTaskSources(List<TaskSource> taskSources)
        {
            bool isUpdate = false;
            foreach (var taskSource in taskSources)
            {
                var target = this._context.TaskSource.Where(x => x.Guid == taskSource.Guid).SingleOrDefault();

                if (target != null)
                {
                    target.CodeName = taskSource.CodeName;
                    target.DisplayName = taskSource.DisplayName;
                    target.IsActive = taskSource.IsActive;
                    target.SetUpdatedDate();
                    isUpdate = true;
                }
            }

            if (isUpdate)
            {
                this._context.SaveChanges();
            }
        }
    }
}
