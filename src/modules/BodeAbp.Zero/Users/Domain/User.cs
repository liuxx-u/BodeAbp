using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BodeAbp.Zero.Permissions.Domain;
using BodeAbp.Zero.Roles.Domain;
using BodeAbp.Zero.Settings.Domain;
using Microsoft.AspNet.Identity;

namespace BodeAbp.Zero.Users.Domain
{
    /// <summary>
    /// 实体——用户
    /// </summary>
    [Table("Zero_User")]
    public class User : FullAuditedEntity<long>, IUser<long>, IPassivable
    {
        #region 常量

        /// <summary>
        /// ÓÃ»§Ãû <see cref="UserName"/> ×î´ó³¤¶È
        /// </summary>
        public const int MaxUserNameLength = 32;

        /// <summary>
        /// ÓÊÏä <see cref="EmailAddress"/> ×î´ó³¤¶È
        /// </summary>
        public const int MaxEmailAddressLength = 256;

        /// <summary>
        /// ÊÖ»úºÅ<see cref="PhoneNo"/>×î´ó³¤¶È
        /// </summary>
        public const int MaxPhoneNoLength = 32;

        /// <summary>
        /// ¹ÜÀíÔ±Ä¬ÈÏÓÃ»§Ãû
        /// </summary>
        public const string AdminUserName = "admin";

        /// <summary>
        /// Ãû³Æ <see cref="Name"/> ×î´ó³¤¶È
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// ÃÜÂë <see cref="Password"/> ×î´ó³¤¶È
        /// </summary>
        public const int MaxPasswordLength = 128;

        /// <summary>
        ///  ÓÃ»§ÊäÈëÃÜÂë<see cref="Password"/> ×î´ó³¤¶È
        /// </summary>
        public const int MaxPlainPasswordLength = 32;

        /// <summary>
        /// Èý·½µÇÂ¼Ô´Ãû³Æ <see cref="AuthenticationSource"/> ×î´ó³¤¶È.
        /// </summary>
        public const int MaxAuthenticationSourceLength = 64;

        /// <summary>
        /// Ä¬ÈÏµÇÂ¼ÃÜÂë
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
        /// 三方认证源
        /// Default: null.
        /// </summary>
        [MaxLength(MaxAuthenticationSourceLength)]
        public string AuthenticationSource { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(MaxNameLength)]
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string PayPassword { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [StringLength(MaxPhoneNoLength)]
        public string PhoneNo { get; set; }

        /// <summary>
        /// 邮箱地址
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
        /// 头像
        /// </summary>
        public string HeadPic { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactNo { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// CC豆
        /// </summary>
        public decimal CcCoin { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否完善资料
        /// </summary>
        public bool IsnoPerfectInfor { get; set; }

        /// <summary>
        /// 外部登录信息集合
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserExternalLogin> Logins { get; set; }

        /// <summary>
        /// 角色集合
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// 权限集合
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserPermission> Permissions { get; set; }

        /// <summary>
        /// 用户设置
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

        #region 重载

        /// <summary>
        /// 重载ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[User {0}] {1}", Id, UserName);
        }

        #endregion
    }

    public enum Sex
    {
        男 = 1,
        女 = 2
    }

    public enum UserType
    {
        平台用户 = 1,
        系统管理员 = 2,
        商家 = 3
    }
}