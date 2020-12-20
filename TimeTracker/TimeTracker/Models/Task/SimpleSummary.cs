using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.Models.Task
{
    public class SimpleSummary
    {
        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime Date { get; set; }

        public double ConsumeTime
        {
            get{
                return this.IsLeave ? this._LimitWorkTime : this._ConsumeTime;
            }
        }

        public double Overtime
        {
            get
            {
                return this.IsLeave ? 0 :
                    this.ConsumeTime - this._LimitWorkTime > 0 ? this.ConsumeTime - this._LimitWorkTime : 0;
            }
        }

        public bool IsLeave { get; set; } = false;

        protected double _ConsumeTime { get; set; }

        protected double _LimitWorkTime { get; set; }

        public SimpleSummary(){}

        public SimpleSummary(DateTime date, double consumeTime, double limitWorkTime, bool isLeave)
        {
            this.Date = date;
            this._ConsumeTime = consumeTime;
            this._LimitWorkTime = limitWorkTime;
            this.IsLeave = isLeave;
        }
    }
}
