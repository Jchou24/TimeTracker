using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracker.DAL;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.Models.Task
{
    public class UpdateTaskCol
    {
        public Guid OwnerGuid { get; set; }

        /// <summary>
        /// Task guid
        /// </summary>
        public Guid Guid { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime Date { get; set; }

        public string RelatedKey { get; set; }

        public object Value { get; set; }

        public bool UpdateEntity(ApplicationDbContext context)
        {
            var task = context.Task.Where(x => x.Guid == this.Guid).SingleOrDefault();

            if (task == null)
            {
                return false;
            }

            switch (this.RelatedKey)
            {
                case nameof(RelatedKeys.consumeTime):
                    double consumeTime;

                    if (!double.TryParse(this.Value.ToString(), out consumeTime))
                    {
                        return false;
                    }
                    task.ConsumeTime = consumeTime;
                    return true;

                case nameof(RelatedKeys.taskType):
                    Guid taskTypeGuid;
                    if (!Guid.TryParse(this.Value.ToString(), out taskTypeGuid))
                    {
                        return false;
                    }

                    var taskType = context.TaskType.SingleOrDefault(x => x.Guid == taskTypeGuid);
                    if (taskType == null)
                    {
                        return false;
                    }

                    task.TaskType = taskType;
                    return true;

                case nameof(RelatedKeys.taskSource):
                    Guid taskSourceGuid;
                    if (!Guid.TryParse(this.Value.ToString(), out taskSourceGuid))
                    {
                        return false;
                    }

                    var taskSource = context.TaskSource.Single(x => x.Guid == taskSourceGuid);
                    if (taskSource == null)
                    {
                        return false;
                    }

                    task.TaskSource = taskSource;
                    return true;

                case nameof(RelatedKeys.taskName):
                    task.TaskName = this.Value.ToString();
                    return true;

                case nameof(RelatedKeys.taskContent):
                    task.TaskContent = this.Value.ToString();
                    return true;

                default:
                    return false;
            }
        }
    }

    public enum RelatedKeys
    {
        consumeTime,
        taskType,
        taskSource,
        taskName,
        taskContent,
    }
}
