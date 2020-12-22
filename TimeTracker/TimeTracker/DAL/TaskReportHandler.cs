using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.Helper.Extensions;
using TimeTracker.Models.Task;

namespace TimeTracker.DAL
{
    public class TaskReportHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public TaskReportHandler(ApplicationDbContext context, ILogger<TaskReportHandler> logger)
        {
            this._context = context;

            _logger = logger;
        }

        public double GetLimitWorkTime()
        {
            return this._context.DayWorkLimitTime
                .OrderByDescending(x => x.CreatedDate)
                .Cacheable()
                .FirstOrDefault()
                .LimitWorkTime;
        }

        protected IQueryable<Task> _SelectTask(QueryPeopleTasks queryPeopleTasks, bool? isLeave = null)
        {
            var reault = this._context.Task.Include(x => x.TaskDay)
                .Where( x => queryPeopleTasks.OwnerGuids.Contains(x.TaskDay.User.Guid) &&
                    queryPeopleTasks.StartDate.Date <= x.Date.Date &&
                    x.Date.Date <= queryPeopleTasks.EndDate.Date);

            if (isLeave != null)
            {
                reault = reault.Where(x => x.TaskDay.IsLeave == isLeave);
            }

            return reault.AsNoTracking().Cacheable();
        }

        /// <summary>
        /// OwnerGuids must be one, otherwise the logic of "overtime", "leave" will go wrong.
        /// </summary>
        /// <param name="queryPeopleTasks"></param>
        /// <returns></returns>
        protected IEnumerable<PersonSummary> _GetPersonSummary(QueryTasks queryTasks)
        {
            var dayWorkLimitTime = this.GetLimitWorkTime();
            var queryPeopleTasks = new QueryPeopleTasks(queryTasks);

            var regularSummary = this._SelectTask(queryPeopleTasks, false)
                        .Select(x => new { Date = x.Date, ConsumeTime = x.ConsumeTime })
                        .AsEnumerable()
                        .GroupBy(x => x.Date.Date)
                        .Select(x => new PersonSummary(x.Key.Date, x.Sum(y => y.ConsumeTime), dayWorkLimitTime, false));

            var leaveSummary = this._context.TaskDay.Where(
                            x => queryPeopleTasks.OwnerGuids.Contains(x.User.Guid) &&
                            queryPeopleTasks.StartDate.Date <= x.Date.Date &&
                            x.Date.Date <= queryPeopleTasks.EndDate.Date && 
                            x.IsLeave == true)
                        .Cacheable()
                        .AsNoTracking()
                        .AsEnumerable()
                        .Select(x => new PersonSummary(x.Date, 0, dayWorkLimitTime, true));

            var result = new List<PersonSummary>();
            result.AddRange(regularSummary.ToList());
            result.AddRange(leaveSummary.ToList());
            result.Sort((x, y) => x.Date.Date.CompareTo(y.Date.Date));

            return result;
        }

        public IEnumerable<MultiSimpleSummary> GetTaskTimeSummaryDetail(QueryPeopleTasks queryPeopleTasks)
        {
            var summaryDict = new Dictionary<DateTime, MultiSimpleSummary>();
            foreach (var date in queryPeopleTasks.StartDate.EachDay(queryPeopleTasks.EndDate))
            {
                summaryDict[date] = new MultiSimpleSummary() { Date = date.Date };
            }

            foreach (var OwnerGuid in queryPeopleTasks.OwnerGuids)
            {
                var queryTasks = new QueryTasks()
                {
                    OwnerGuid = OwnerGuid,
                    StartDate = queryPeopleTasks.StartDate,
                    EndDate = queryPeopleTasks.EndDate
                };
                var simpleSummary = this._GetPersonSummary(queryTasks);

                foreach (var row in simpleSummary)
                {
                    summaryDict[row.Date.Date].AddRow(row);
                }
            }

            var taskTimeSummaryDetail = summaryDict.Values.ToList();
            taskTimeSummaryDetail.Sort((x, y) => x.Date.Date.CompareTo(y.Date.Date));

            return taskTimeSummaryDetail;
        }

        public IEnumerable<SimpleSummary> GetSimpleSummary(QueryPeopleTasks queryPeopleTasks)
        {
            IEnumerable<SimpleSummary> simpleSummary;
            if (queryPeopleTasks.OwnerGuids.Count == 1)
            {
                var queryTasks = new QueryTasks()
                {
                    OwnerGuid = queryPeopleTasks.OwnerGuids.FirstOrDefault(),
                    StartDate = queryPeopleTasks.StartDate,
                    EndDate = queryPeopleTasks.EndDate
                };
                simpleSummary = this._GetPersonSummary(queryTasks).Select(x => new SimpleSummary(x));
            }
            else
            {
                var taskTimeSummaryDetail = this.GetTaskTimeSummaryDetail(queryPeopleTasks);
                simpleSummary = taskTimeSummaryDetail.Select(x => new SimpleSummary(x));
            }
            return simpleSummary;
        }

        public IEnumerable<PieRow> GetTaskTypeSummary(QueryPeopleTasks queryPeopleTasks)
        {
            var result = this._SelectTask(queryPeopleTasks, false)
                .Select(x => new { Key = x.TaskType.Guid, TaskTypeDisplayName = x.TaskType.DisplayName, ConsumeTime = x.ConsumeTime })
                .AsEnumerable()
                .GroupBy(x => x.Key).ToList()
                .Select(x => new PieRow()
                {
                    Name = x.FirstOrDefault().TaskTypeDisplayName,
                    Value = x.Sum(y => y.ConsumeTime)
                });

            return result;
        }

        public IEnumerable<PieRow> GetTaskSourceSummary(QueryPeopleTasks queryPeopleTasks)
        {
            var result = this._SelectTask(queryPeopleTasks, false)
                .Select(x => new { Key = x.TaskSource.Guid, TaskSourceDisplayName = x.TaskSource.DisplayName, ConsumeTime = x.ConsumeTime })
                .AsEnumerable()
                .GroupBy(x => x.Key).ToList()
                .Select(x => new PieRow()
                {
                    Name = x.FirstOrDefault().TaskSourceDisplayName,
                    Value = x.Sum(y => y.ConsumeTime)
                });

            return result;
        }

        public IEnumerable<PieRow> GetTaskTimeSummary(QueryPeopleTasks queryPeopleTasks)
        {
            var taskTimeSummaryDetail = GetTaskTimeSummaryDetail(queryPeopleTasks);

            var totalConsumeTime = taskTimeSummaryDetail.Aggregate(0.0, (total, next) => total + next.TotalConsumeTime);
            var totalOverTime = taskTimeSummaryDetail.Aggregate(0.0, (total, next) => total + next.TotalOvertime);
            var totalLeaveTime = taskTimeSummaryDetail.Aggregate(0.0, (total, next) => total + next.TotalLeave);
            var totalRegularTime = totalConsumeTime - totalOverTime - totalLeaveTime;

            var result = new List<PieRow>();
            result.Add(new PieRow() { Name = "Regular", Value = totalRegularTime });
            result.Add(new PieRow() { Name = "Over", Value = totalOverTime });
            result.Add(new PieRow() { Name = "Leave", Value = totalLeaveTime });

            return result;
        }
    }
}
