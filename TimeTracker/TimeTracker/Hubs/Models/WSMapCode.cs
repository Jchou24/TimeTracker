using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Hubs.Models
{
    public enum WSMapCode
    {
        GetUserInfo = 0,
        TaskEditorCreateTask = 1,
        TaskEditorDeleteTasks = 2,
        TaskEditorUpdateTaskRowOrder = 3,
        TaskEditorUpdateTaskCol = 4,
        TaskEditorUpdateIsLeave = 5,
    }
}
