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


        public double AvgConsumeTime
        {
            get => this.ConsumeTime.Count > 0 ? this.ConsumeTime.Average() : Double.NaN;
        }
        public double AvgOvertime
        {
            get => this.Overtime.Count > 0 ? this.Overtime.Average() : Double.NaN;
        }


        public double MedianConsumeTime
        {
            get => this.ConsumeTime.Count > 0 ? this.ConsumeTime.Median() : Double.NaN;
        }
        public double MedianOvertime
        {
            get => this.Overtime.Count > 0 ? this.Overtime.Median() : Double.NaN;
        }


        public double MinConsumeTime
        {
            get => this.ConsumeTime.Minimum();
        }
        public double MinOvertime
        {
            get => this.Overtime.Minimum();
        }


        public double MaxConsumeTime
        {
            get => this.ConsumeTime.Maximum();
        }
        public double MaxOvertime
        {
            get => this.Overtime.Maximum();
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
