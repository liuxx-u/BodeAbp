using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Zero.Navigations.Domain
{
    /// <summary>
    /// 角色导航关系
    /// </summary>
    public class RoleNavigation : CreationAuditedEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 导航Id
        /// </summary>
        public int NavigationId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleNavigation() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="navigationId">导航Id</param>
        public RoleNavigation(int roleId,int navigationId)
        {
            RoleId = roleId;
            NavigationId = navigationId;
        }
    }
}
