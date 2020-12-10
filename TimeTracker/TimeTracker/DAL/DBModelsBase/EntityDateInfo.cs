using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TimeTracker.DAL.Attributes;

namespace TimeTracker.DAL.DBModelsBase
{
    public class EntityDateInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SqlDefaultValue(DefaultValue = "getutcdate()")]
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SqlDefaultValue(DefaultValue = "getutcdate()")]
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }

        public void SetUpdatedDate()
        { 
            this.UpdatedDate = DateTime.UtcNow;
        }
    }
}
