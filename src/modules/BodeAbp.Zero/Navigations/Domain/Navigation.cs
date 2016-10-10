using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Zero.Navigations.Domain
{
    /// <summary>
    /// 导航信息
    /// </summary>
    [Table("Zero_Navigation")]
    public class Navigation : FullAuditedEntity
    {
        #region 常量
        
        /// <summary>
        /// 菜单名称 <see cref="Name"/> 最大长度.
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// 菜单Url地址 <see cref="Name"/> 最大长度.
        /// </summary>
        public const int MaxUrlLength = 64;

        #endregion

        #region 属性

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        [StringLength(MaxUrlLength)]
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
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        public NavigationType NavigationType { get; set; }

        #endregion

    }
    /// <summary>
    /// 表示导航访问类型的枚举
    /// </summary>
    public enum NavigationType
    {
        /// <summary>
        /// 游客可访问
        /// </summary>
        [Description("游客访问")]
        Anonymouse = 0,

        /// <summary>
        /// 登录用户可访问
        /// </summary>
        [Description("登陆访问")]
        Logined = 1,

        /// <summary>
        /// 指定角色可访问
        /// </summary>
        [Description("角色访问")]
        RoleLimit = 2
    }
}
