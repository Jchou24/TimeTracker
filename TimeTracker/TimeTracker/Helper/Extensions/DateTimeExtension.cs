using System;
using System.Collections.Generic;

namespace TimeTracker.Helper.Extensions
{
    public static class DateTimeExtension
    {
        public static IEnumerable<DateTime> EachDay(this DateTime start, DateTime end)
        {
            for (var day = start.Date; day.Date <= end.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
