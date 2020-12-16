using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Task
{
    public class DeleteTasks
    {
        public Guid OwnerGuid { get; set; }

        /// <summary>
        /// yyy-MM-dd
        /// </summary>
        public string Date { get; set; }

        public List<Guid> TaskGuids { get; set; }

    }
}
