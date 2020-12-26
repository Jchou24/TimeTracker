using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TimeTracker.DAL.Attributes;

namespace TimeTracker.DAL.DBModelsBase
{
    public class EntityDateInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue(typeof(DateTime), "")]
        //[SqlDefaultValue(DefaultValue = "getutcdate()")]
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue(typeof(DateTime), "")]
        //[SqlDefaultValue(DefaultValue = "getutcdate()")]
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        public void SetUpdatedDate()
        { 
            this.UpdatedDate = DateTime.UtcNow;
        }
    }
}
