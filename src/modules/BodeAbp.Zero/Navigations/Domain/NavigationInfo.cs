using System.Collections.Generic;

namespace BodeAbp.Zero.Navigations.Domain
{
    /// <summary>
    /// 导航信息
    /// </summary>
    public class NavigationInfo
    {
        public NavigationInfo()
        {
            Children = new List<NavigationInfo>();
        }

        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 父级Id，0表示一级分类
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否关联
        /// </summary>
        public bool IsRelation { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        public NavigationType NavigationType { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string RequiredPermissionName { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ICollection<NavigationInfo> Children { get; set; }
    }
}
