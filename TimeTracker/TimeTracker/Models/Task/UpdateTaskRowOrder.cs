using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models.Task
{
    public class UpdateTaskRowOrder
    {
        public Guid Guid { get; set; }

        public int DisplayOrder { get; set; }
    }
}
