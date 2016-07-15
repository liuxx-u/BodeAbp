using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Activity.Participants.Domain
{
    /// <summary>
    /// 活动参与者
    /// </summary>
    [Table("Activity#Paticipant")]
    public class Paticipant: FullAuditedEntity<long>
    {
        /// <summary>
        /// 是否付费
        /// </summary>
        public bool IsPay { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public long ActivityId { get; set; }
    }
}
