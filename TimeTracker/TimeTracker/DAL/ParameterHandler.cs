using EFCoreSecondLevelCacheInterceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
