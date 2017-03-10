using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.AspNet.Identity;

namespace BodeAbp.Zero.Users.Domain
{
    /// <summary>
    /// 实体——用户关联地区
    /// </summary>
    [Table("Zero_UserRelationRegion")]
    public class UserRelationRegion : Entity<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 地区id
        /// </summary>
        public int RegionId { get; set; }
    }
}
