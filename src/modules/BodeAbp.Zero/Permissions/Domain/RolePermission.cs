using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Zero.Permissions.Domain
{
    /// <summary>
    /// 实体——角色权限
    /// </summary>
    [Table("Zero#RolePermission")]
    public class RolePermission : PermissionBase
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
    }
}
