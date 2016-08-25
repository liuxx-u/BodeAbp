using System;
using Abp.Domain.Entities;
using Microsoft.AspNet.Identity;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Zero.Roles.Domain
{
    /// <summary>
    /// 实体——角色信息
    /// </summary>
    [Table("Zero#Role")]
    public class Role : FullAuditedEntity<int>, IRole<int>
    {
        #region 常量

        /// <summary>
        /// 角色唯一标识符 <see cref="Name"/> 最大长度.
        /// </summary>
        public const int MaxNameLength = 32;
        
        /// <summary>
        /// 角色显示名 <see cref="DisplayName"/> 最大长度.
        /// </summary>
        public const int MaxDisplayNameLength = 64;

        /// <summary>
        /// 管理角色
        /// </summary>
        public const string AdminRoleName = "admin";

        #endregion

        #region 属性

        /// <summary>
        /// 角色唯一标识符
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// 角色显示名.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public string DisplayName { get; set; }
                
        /// <summary>
        /// 是否静态角色
        /// 静态属性不能删除，不能修改
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 是否默认属性
        /// 创建用户时自动添加默认角色
        /// </summary>
        public bool IsDefault { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数.
        /// </summary>
        public Role()
        {
            Name = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="displayName">角色显示名</param>
        public Role(string displayName)
            : this()
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="name">角色唯一标识码</param>
        /// <param name="displayName">角色显示名</param>
        public Role(string name, string displayName)
            : this(displayName)
        {
            Name = name;
        }

        #endregion

        #region 重载

        /// <summary>
        /// 重写ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Role {0}, Name={1}]", Id, Name);
        }

        #endregion
    }
}
