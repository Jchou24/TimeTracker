using System.ComponentModel.DataAnnotations;
using TimeTracker.DAL.DBModels.Task;

namespace TimeTracker.DAL.DBModelsBase
{
    public class EntityTask: EntityTaskBase
    {
        /// <summary>
        /// 顯示順序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 工作事項類型
        /// </summary>
        public TaskType TaskType { get; set; }

        /// <summary>
        /// 工作事項來源
        /// </summary>
        public TaskSource TaskSource { get; set; }

        /// <summary>
        /// 工作事項名稱
        /// </summary>
        /// [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string TaskName { get; set; }

        /// <summary>
        /// 工作內容
        /// </summary>
        /// [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string TaskContent { get; set; }
    }
}
