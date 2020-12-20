using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.Models.Task;

namespace TimeTracker.DAL
{
    public class TaskReportHandler
    {
        private readonly ApplicationDbContext _context;

        public TaskReportHandler(ApplicationDbContext context)
        {
            this._context = context;
        }

        public double GetLimitWorkTime()
        {
            return this._context.DayWorkLimitTime.OrderByDescending(x => x.CreatedDate).Take(1).ToList()[0].LimitWorkTime;
        }

        public IQueryable<Task> SelectTask(QueryTasks queryTasks)
        {
            return this._context.Task.Include(x => x.TaskDay)
                .Where(x =>
                    x.TaskDay.User.Guid == queryTasks.OwnerGuid &&
                    queryTasks.StartDate.Date <= x.Date.Date &&
                    x.Date.Date <= queryTasks.EndDate.Date)
                .AsNoTracking();
        }

        public IEnumerable<SimpleSummary> GetSimpleSummary(QueryTasks queryTasks)
        {
            var dayWorkLimitTime = this.GetLimitWorkTime();

            var regularSummary = this.SelectTask(queryTasks).Where(x => x.TaskDay.IsLeave == false)
                        .Select(x => new { Date = x.Date, ConsumeTime = x.ConsumeTime })
                        .AsEnumerable()
                        .GroupBy(x => x.Date.Date)
                        .Select(x => new SimpleSummary(x.Key.Date, x.Sum(y => y.ConsumeTime), dayWorkLimitTime, false));

            var leaveSummary = this._context.TaskDay
                        .Where( x => 
                            x.User.Guid == queryTasks.OwnerGuid &&
                            queryTasks.StartDate.Date <= x.Date.Date &&
                            x.Date.Date <= queryTasks.EndDate.Date &&
                            x.IsLeave == true)
                        .AsEnumerable()
                        .Select(x => new SimpleSummary(x.Date, 0, dayWorkLimitTime, true));

            var result = new List<SimpleSummary>();
            result.AddRange(regularSummary.ToList());
            result.AddRange(leaveSummary.ToList());
            result.Sort((x, y) => x.Date.Date.CompareTo(y.Date.Date));

            return result;
        }

        public IEnumerable<PieRow> GetTaskTypeSummary(QueryTasks queryTasks)
        {
            var result = this.SelectTask(queryTasks).Where(x => x.TaskDay.IsLeave == false)
                .Select(x => new { Key = x.TaskType.Guid, TaskTypeDisplayName = x.TaskType.DisplayName, ConsumeTime = x.ConsumeTime })
                .AsEnumerable()
                .GroupBy(x => x.Key).ToList()
                .Select(x => new PieRow()
                {
                    Name = x.Take(1).ToList()[0].TaskTypeDisplayName,
                    Value = x.Sum(y => y.ConsumeTime)
                });

            return result;
        }

        public IEnumerable<PieRow> GetTaskSourceSummary(QueryTasks queryTasks)
        {
            var result = this.SelectTask(queryTasks).Where(x => x.TaskDay.IsLeave == false)
                .Select(x => new { Key = x.TaskSource.Guid, TaskSourceDisplayName = x.TaskSource.DisplayName, ConsumeTime = x.ConsumeTime })
                .AsEnumerable()
                .GroupBy(x => x.Key).ToList()
                .Select(x => new PieRow()
                {
                    Name = x.Take(1).ToList()[0].TaskSourceDisplayName,
                    Value = x.Sum(y => y.ConsumeTime)
                });

            return result;
        }

        public IEnumerable<PieRow> GetTaskTimeSummary(QueryTasks queryTasks)
        {
            var dayWorkLimitTime = this.GetLimitWorkTime();
            var simpleSummary = this.GetSimpleSummary(queryTasks);

            var totalConsumeTime = simpleSummary.Aggregate(0.0, (total, next) => total + next.ConsumeTime);
            var totalOverTime = simpleSummary.Where(x => x.IsLeave == false).AsEnumerable().Aggregate(0.0, (total, next) => total + next.Overtime);
            var totalLeaveTime = simpleSummary.Where(x => x.IsLeave == true).AsEnumerable().Aggregate(0.0, (total, next) => total + dayWorkLimitTime);
            var totalRegularTime = totalConsumeTime - totalOverTime - totalLeaveTime;

            var result = new List<PieRow>();
            result.Add(new PieRow() { Name = "Regular Time", Value = totalRegularTime });
            result.Add(new PieRow() { Name = "Over Time", Value = totalOverTime });
            result.Add(new PieRow() { Name = "Leave Time", Value = totalLeaveTime });

            return result;
        }
    }
}
