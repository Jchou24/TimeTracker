using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models
{
    public class UpdatePassword
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
    }
}
