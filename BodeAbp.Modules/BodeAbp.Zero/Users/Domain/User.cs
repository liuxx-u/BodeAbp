using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using BodeAbp.Zero.Permissions.Domain;
using BodeAbp.Zero.Roles.Domain;
using BodeAbp.Zero.Settings.Domain;
using Microsoft.AspNet.Identity;

namespace BodeAbp.Zero.Users.Domain
{
    /// <summary>
    /// 实体——用户信息
    /// </summary>
    [Table("Zero#User")]
    public class User : FullAuditedEntity<long>, IUser<long>, IPassivable
    {
        #region 常量
        
        /// <summary>
        /// 用户名 <see cref="UserName"/> 最大长度
        /// </summary>
        public const int MaxUserNameLength = 32;

        /// <summary>
        /// 邮箱 <see cref="EmailAddress"/> 最大长度
        /// </summary>
        public const int MaxEmailAddressLength = 256;

        /// <summary>
        /// 手机号<see cref="PhoneNo"/>最大长度
        /// </summary>
        public const int MaxPhoneNoLength = 32;

        /// <summary>
        /// 管理员默认用户名
        /// </summary>
        public const string AdminUserName = "admin";

        /// <summary>
        /// 名称 <see cref="Name"/> 最大长度
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// 密码 <see cref="Password"/> 最大长度
        /// </summary>
        public const int MaxPasswordLength = 128;

        /// <summary>
        ///  用户输入密码<see cref="Password"/> 最大长度
        /// </summary>
        public const int MaxPlainPasswordLength = 32;

        /// <summary>
        /// 三方登录源名称 <see cref="AuthenticationSource"/> 最大长度.
        /// </summary>
        public const int MaxAuthenticationSourceLength = 64;

        /// <summary>
        /// 默认登录密码
        /// </summary>
        public const string DefaultPassword = "bode123456";

        #endregion

        #region 属性

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// Authorization source name.
        /// 三方登录源名称
        /// Default: null.
        /// </summary>
        [MaxLength(MaxAuthenticationSourceLength)]
        public string AuthenticationSource { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        [StringLength(MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(MaxPhoneNoLength)]
        public string PhoneNo { get; set; }

        /// <summary>
        /// 邮箱
        /// Email address must be unique for it's tenant.
        /// </summary>
        [StringLength(MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 是否验证邮箱 <see cref="EmailAddress"/>.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// 是否验证手机<see cref="PhoneNo"/>
        /// </summary>
        public bool IsPhoneNoConfirm { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 用户自定义数据（json格式）
        /// </summary>
        public string CustomData { get; set; }

        /// <summary>
        /// 用户登录集合
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserExternalLogin> Logins { get; set; }

        /// <summary>
        /// 用户角色集合
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// 用户权限集合
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserPermission> Permissions { get; set; }

        /// <summary>
        /// 用户设置集合
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<Setting> Settings { get; set; }

        #endregion

        #region 构造函数
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public User()
        {
            IsActive = true;
        }

        #endregion

        #region 公共方法
        
        /// <summary>
        /// 创建用户标识符 <see cref="UserIdentifier"/>
        /// </summary>
        /// <returns></returns>
        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(Id);
        }

        #endregion

        #region 重载

        /// <summary>
        /// 重写ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[User {0}] {1}", Id, UserName);
        }

        #endregion
    }
}