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
    /// 实体——用户
    /// </summary>
    [Table("Zero_User")]
    public class User : FullAuditedEntity<long>, IUser<long>, IPassivable
    {
        #region ³£Á¿
        
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

        #region ÊôÐÔ

        /// <summary>
        /// ÓÃ»§Ãû
        /// </summary>
        [Required]
        [MaxLength(MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// Authorization source name.
        /// Èý·½µÇÂ¼Ô´Ãû³Æ
        /// Default: null.
        /// </summary>
        [MaxLength(MaxAuthenticationSourceLength)]
        public string AuthenticationSource { get; set; }

        /// <summary>
        /// Ãû³Æ
        /// </summary>
        [StringLength(MaxNameLength)]
        public string NickName { get; set; }

        /// <summary>
        /// ÓÃ»§ÃÜÂë
        /// </summary>
        [Required]
        [StringLength(MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// ÊÖ»úºÅÂë
        /// </summary>
        [StringLength(MaxPhoneNoLength)]
        public string PhoneNo { get; set; }

        /// <summary>
        /// ÓÊÏä
        /// Email address must be unique for it's tenant.
        /// </summary>
        [StringLength(MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// ×îºóµÇÂ¼Ê±¼ä
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// ÊÇ·ñÑéÖ¤ÓÊÏä <see cref="EmailAddress"/>.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// ÊÇ·ñÑéÖ¤ÊÖ»ú<see cref="PhoneNo"/>
        /// </summary>
        public bool IsPhoneNoConfirm { get; set; }

        /// <summary>
        /// ÊÇ·ñ¼¤»î
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// ÓÃ»§×Ô¶¨ÒåÊý¾Ý£¨json¸ñÊ½£©
        /// </summary>
        public string CustomData { get; set; }

        /// <summary>
        /// ÓÃ»§µÇÂ¼¼¯ºÏ
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserExternalLogin> Logins { get; set; }

        /// <summary>
        /// ÓÃ»§½ÇÉ«¼¯ºÏ
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// ÓÃ»§È¨ÏÞ¼¯ºÏ
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserPermission> Permissions { get; set; }

        /// <summary>
        /// ÓÃ»§ÉèÖÃ¼¯ºÏ
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<Setting> Settings { get; set; }

        #endregion

        #region ¹¹Ôìº¯Êý
        
        /// <summary>
        /// ¹¹Ôìº¯Êý
        /// </summary>
        public User()
        {
            IsActive = true;
        }

        #endregion

        #region ¹«¹²·½·¨
        

        #endregion

        #region ÖØÔØ

        /// <summary>
        /// ÖØÐ´ToString()·½·¨
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[User {0}] {1}", Id, UserName);
        }

        #endregion
    }
}