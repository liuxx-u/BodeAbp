using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Zero.Users.Domain;

namespace BodeAbp.Zero.Application.Users.Dtos
{
    public abstract class ValidateCodeDto
    {
        /// <summary>
        /// 验证码Key(手机或邮箱)
        /// </summary>

        [Required]
        [MaxLength(ValidateCode.MaxCodeKeyLength)]
        public string CodeKey { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
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

    [AutoMapTo(typeof(ValidateCode))]
    public class ValidateCodeInput : ValidateCodeDto, IInputDto
    {

    }
}
