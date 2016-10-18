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
    /// ʵ�塪���û���Ϣ
    /// </summary>
    [Table("Zero_User")]
    public class User : FullAuditedEntity<long>, IUser<long>, IPassivable
    {
        #region ����
        
        /// <summary>
        /// �û��� <see cref="UserName"/> ��󳤶�
        /// </summary>
        public const int MaxUserNameLength = 32;

        /// <summary>
        /// ���� <see cref="EmailAddress"/> ��󳤶�
        /// </summary>
        public const int MaxEmailAddressLength = 256;

        /// <summary>
        /// �ֻ���<see cref="PhoneNo"/>��󳤶�
        /// </summary>
        public const int MaxPhoneNoLength = 32;

        /// <summary>
        /// ����ԱĬ���û���
        /// </summary>
        public const string AdminUserName = "admin";

        /// <summary>
        /// ���� <see cref="Name"/> ��󳤶�
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// ���� <see cref="Password"/> ��󳤶�
        /// </summary>
        public const int MaxPasswordLength = 128;

        /// <summary>
        ///  �û���������<see cref="Password"/> ��󳤶�
        /// </summary>
        public const int MaxPlainPasswordLength = 32;

        /// <summary>
        /// ������¼Դ���� <see cref="AuthenticationSource"/> ��󳤶�.
        /// </summary>
        public const int MaxAuthenticationSourceLength = 64;

        /// <summary>
        /// Ĭ�ϵ�¼����
        /// </summary>
        public const string DefaultPassword = "bode123456";

        #endregion

        #region ����

        /// <summary>
        /// �û���
        /// </summary>
        [Required]
        [MaxLength(MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// Authorization source name.
        /// ������¼Դ����
        /// Default: null.
        /// </summary>
        [MaxLength(MaxAuthenticationSourceLength)]
        public string AuthenticationSource { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [StringLength(MaxNameLength)]
        public string NickName { get; set; }

        /// <summary>
        /// �û�����
        /// </summary>
        [Required]
        [StringLength(MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// �ֻ�����
        /// </summary>
        [StringLength(MaxPhoneNoLength)]
        public string PhoneNo { get; set; }

        /// <summary>
        /// ����
        /// Email address must be unique for it's tenant.
        /// </summary>
        [StringLength(MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// ����¼ʱ��
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// �Ƿ���֤���� <see cref="EmailAddress"/>.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// �Ƿ���֤�ֻ�<see cref="PhoneNo"/>
        /// </summary>
        public bool IsPhoneNoConfirm { get; set; }

        /// <summary>
        /// �Ƿ񼤻�
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// �û��Զ������ݣ�json��ʽ��
        /// </summary>
        public string CustomData { get; set; }

        /// <summary>
        /// �û���¼����
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserExternalLogin> Logins { get; set; }

        /// <summary>
        /// �û���ɫ����
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// �û�Ȩ�޼���
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserPermission> Permissions { get; set; }

        /// <summary>
        /// �û����ü���
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<Setting> Settings { get; set; }

        #endregion

        #region ���캯��
        
        /// <summary>
        /// ���캯��
        /// </summary>
        public User()
        {
            IsActive = true;
        }

        #endregion

        #region ��������
        

        #endregion

        #region ����

        /// <summary>
        /// ��дToString()����
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[User {0}] {1}", Id, UserName);
        }

        #endregion
    }
}