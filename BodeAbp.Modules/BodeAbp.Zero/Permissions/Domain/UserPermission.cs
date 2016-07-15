using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Zero.Permissions.Domain
{
    /// <summary>
    /// 实体——用户权限
    /// </summary>
    [Table("Zero#UserPermission")]
    public class UserPermission : PermissionBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
    }
}
