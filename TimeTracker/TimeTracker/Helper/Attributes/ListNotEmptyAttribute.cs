using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Helper.Attributes
{
    public class ListNotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }
    }
}
