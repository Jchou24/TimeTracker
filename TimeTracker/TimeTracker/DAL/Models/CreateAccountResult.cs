using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.DAL.Models
{
    public enum CreateAccountResult
    {
        Ok = 0,
        AccountExist = 1,
        Fail = 2,
    }
}
