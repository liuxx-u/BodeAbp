using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Forum.Plates.Domain
{
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
