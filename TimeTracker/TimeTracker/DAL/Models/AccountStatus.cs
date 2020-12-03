using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.DAL.Models
{
    public enum AccountStatus
    {
        Uncheck = 0, // 未審核用戶
        Approved = 1, // 合法用戶
        Rejected = 2, // 拒絕用戶
        Suspend = 3, // 停權用戶
    }
}
