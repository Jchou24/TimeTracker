using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracker.DAL.Attributes;

namespace TimeTracker.DAL.Models
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SqlDefaultValueAttribute(DefaultValue = "getutcdate()")]
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SqlDefaultValueAttribute(DefaultValue = "getutcdate()")]
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }

        public void SetUpdatedDate()
        { 
            this.UpdatedDate = DateTime.UtcNow;
        }
    }
}
