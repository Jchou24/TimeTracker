using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.DAL.Attributes;

namespace TimeTracker.DAL.Models
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SqlDefaultValueAttribute(DefaultValue = "getutcdate()")]
        public DateTime CreatedDate { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SqlDefaultValueAttribute(DefaultValue = "getutcdate()")]
        public DateTime UpdatedDate { get; set; }

        public void SetUpdatedDate()
        { 
            this.UpdatedDate = DateTime.UtcNow;
        }
    }
}
