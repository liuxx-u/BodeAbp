using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Zero.Roles.Domain
{
    /// <summary>
    /// 实体——用户角色关系
    /// </summary>
    [Table("Zero#UserRole")]
    public class UserRole: CreationAuditedEntity<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserRole()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleId">角色Id</param>
        public UserRole(long userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
