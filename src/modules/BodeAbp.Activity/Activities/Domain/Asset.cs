using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Asset;

namespace BodeAbp.Activity.Activities.Domain
{
    /// <summary>
    /// 活动资源（图片、视频、文档）
    /// </summary>
    [Table("Activity#Asset")]
    public class Asset : AssetEntity<long>
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public long ActivityId { get; set; }
    }
}
