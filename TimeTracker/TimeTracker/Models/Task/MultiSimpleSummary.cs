using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using TimeTracker.Helper.Attributes;
using MathNet.Numerics.Statistics;

namespace TimeTracker.Models.Task
{
    public class MultiSimpleSummary
    {
        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime Date { get; set; }

        [JsonIgnore]
        public List<double> ConsumeTime { get; set; } = new List<double>();

        [JsonIgnore]
        public List<double> Overtime { get; set; } = new List<double>();

        [JsonIgnore]
        public List<double> Leave { get; set; } = new List<double>();


        public double TotalConsumeTime
        {
            get => this.ConsumeTime.Sum();
        }
        public double TotalOvertime
        {
            get => this.Overtime.Sum();
        }
        public double TotalLeave
        {
            get => this.Leave.Sum();
        }


        public double? AvgConsumeTime
        {
            get
            {
                if (this.ConsumeTime.Count > 0)
                {
                    return this.ConsumeTime.Average();
                }
                return null;
            }
        }
        public double? AvgOvertime
        {
            get
            {
                if (this.Overtime.Count > 0)
                {
                    return this.Overtime.Average();
                }
                return null;
            }
        }


        public double? MedianConsumeTime
        {
            get
            {
                if (this.ConsumeTime.Count > 0)
                {
                    return this.ConsumeTime.Median();
                }
                return null;
            }
        }
        public double? MedianOvertime
        {
            get
            {
                if (this.Overtime.Count > 0)
                {
                    return this.Overtime.Median();
                }
                return null;
            }
        }


        public double? MinConsumeTime
        {
            get
            {
                if (this.ConsumeTime.Count > 0)
                {
                    return this.ConsumeTime.Min();
                }
                return null;
            }
        }
        public double? MinOvertime
        {
            get
            {
                if (this.Overtime.Count > 0)
                {
                    return this.Overtime.Min();
                }
                return null;
            }
        }


        public double? MaxConsumeTime
        {
            get
            {
                if (this.ConsumeTime.Count > 0)
                {
                    return this.ConsumeTime.Max();
                }
                return null;
            }
        }
        public double? MaxOvertime
        {
            get
            {
                if (this.Overtime.Count > 0)
                {
                    return this.Overtime.Max();
                }
                return null;
            }
        }

        public void AddRow(PersonSummary row)
        {
            if (row.IsLeave)
            {
                this.Leave.Add(row.ConsumeTime);
                this.ConsumeTime.Add(row.ConsumeTime);
            }
            else
            {
                this.ConsumeTime.Add(row.ConsumeTime);
                this.Overtime.Add(row.Overtime);
            }
        }

    }
}
