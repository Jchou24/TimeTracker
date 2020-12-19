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
            var dayWorkLimitTime = this._context.DayWorkLimitTime.OrderByDescending(x => x.CreatedDate).Take(1).ToList()[0];

            var result = this.SelectTask(queryTasks)
                        .Select(x => new { Date = x.Date, ConsumeTime = x.ConsumeTime })
                        .AsEnumerable()
                        .GroupBy(x => x.Date.Date)
                        .Select(x => new SimpleSummary(x.Key.Date, x.Sum(y => y.ConsumeTime), dayWorkLimitTime.LimitWorkTime));

            return result;
        }
    }
}
