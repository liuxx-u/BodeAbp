using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Zero.Permissions.Domain
{
    /// <summary>
    /// 实体——角色权限
    /// </summary>
    [Table("Zero_RolePermission")]
    public class RolePermission : PermissionBase
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// true为选中的权限，false为选中的父级权限
        /// </summary>
        public bool IsCheckd { get; set; }
    }
}
