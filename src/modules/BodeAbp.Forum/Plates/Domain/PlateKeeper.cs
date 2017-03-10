using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Forum.Plates.Domain
{
    [Table("Forum_Keeper")]
    public class PlateKeeper : CreationAuditedEntity
    {
        /// <summary>
        /// 管理员Id
        /// </summary>
        public long KeeperId { get; set; }

        /// <summary>
        /// 板块Id
        /// </summary>
        public int PlateId { get; set; }
        
        /// <summary>
        /// 是否版主
        /// </summary>
        public bool isModerator { get; set; } 
    }
}
