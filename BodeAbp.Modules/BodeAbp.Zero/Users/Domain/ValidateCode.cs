using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodeAbp.Zero.Users.Domain
{
    /// <summary>
    /// 实体——验证码
    /// </summary>
    [Table("Zero#ValidateCode")]
    public class ValidateCode : CreationAuditedEntity<long>
    {
        /// <summary>
        /// 验证码 <see cref="Code"/> 最大长度
        /// </summary>
        public const int MaxCodeLength = 16;

        /// <summary>
        /// 验证码 <see cref="CodeKey"/> 最大长度
        /// </summary>
        public const int MaxCodeKeyLength = 256;

        /// <summary>
        /// 验证码Key(手机或邮箱)
        /// </summary>
        [Required]
        [MaxLength(MaxCodeKeyLength)]
        public string CodeKey { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        [MaxLength(MaxCodeLength)]
        public string Code { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public CodeType CodeType { get; set; }

        /// <summary>
        /// 验证方式
        /// </summary>
        public ValidateType ValidateType { get; set; }
    }

    public enum CodeType
    {
        注册 = 1,
        找回密码 = 2,
        更换用户名 = 3,
        动态登录 = 4
    }

    public enum ValidateType
    {
        手机 = 1,
        邮箱 = 2
    }
}
