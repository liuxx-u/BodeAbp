using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Zero.Users.Domain
{
    /// <summary>
    /// 实体——外部登录信息
    /// </summary>
    [Table("Zero#UserExternalLogin")]
    public class UserExternalLogin : Entity<long>
    {
        #region 常量

        /// <summary>
        /// 三方登录源 <see cref="LoginProvider"/> 最大长度.
        /// </summary>
        public const int MaxLoginProviderLength = 128;

        /// <summary>
        /// 三方登录Key <see cref="ProviderKey"/> 最大长度.
        /// </summary>
        public const int MaxProviderKeyLength = 256;

        #endregion

        #region 属性
        
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 三方登录源
        /// </summary>
        [Required]
        [MaxLength(MaxLoginProviderLength)]
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// 三方登录Key
        /// </summary>
        [Required]
        [MaxLength(MaxProviderKeyLength)]
        public virtual string ProviderKey { get; set; }

        #endregion
    }
}
