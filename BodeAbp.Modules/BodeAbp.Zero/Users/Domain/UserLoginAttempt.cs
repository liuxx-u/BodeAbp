using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace BodeAbp.Zero.Users.Domain
{
    /// <summary>
    /// 实体——用户登录信息
    /// </summary>
    [Table("Zero#UserLoginAttempt")]
    public class UserLoginAttempt : Entity<long>, IHasCreationTime
    {
        #region 常量
        
        /// <summary>
        /// Max length of the <see cref="UserName"/> property.
        /// </summary>
        public const int MaxUserNameLength = 255;

        /// <summary>
        /// Maximum length of <see cref="ClientIpAddress"/> property.
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="ClientName"/> property.
        /// </summary>
        public const int MaxClientNameLength = 128;

        /// <summary>
        /// Maximum length of <see cref="BrowserInfo"/> property.
        /// </summary>
        public const int MaxBrowserInfoLength = 256;

        #endregion

        #region 属性

        /// <summary>
        /// 用户Id，用户名验证验证失败为NULL
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(MaxUserNameLength)]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 登录客户端IP.
        /// </summary>
        [MaxLength(MaxClientIpAddressLength)]
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// 登录客户端设备名称.
        /// </summary>
        [MaxLength(MaxClientNameLength)]
        public virtual string ClientName { get; set; }

        /// <summary>
        /// 浏览器信息.
        /// </summary>
        [MaxLength(MaxBrowserInfoLength)]
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// 登录结果.
        /// </summary>
        public virtual AbpLoginResultType Result { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        #endregion

        #region 构造函数
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserLoginAttempt()
        {
            CreationTime = Clock.Now;
        }

        #endregion
    }

    /// <summary>
    /// 用户登录结果
    /// </summary>
    public enum AbpLoginResultType : byte
    {
        Success = 1,

        InvalidUserName,

        InvalidPassword,

        UserIsNotActive,

        EmailIsNotConfirmed,

        PhoneNoIsNotConfirm,

        UnknownExternalLogin
    }
}
