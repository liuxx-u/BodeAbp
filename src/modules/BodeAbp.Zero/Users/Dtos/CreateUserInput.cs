using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Zero.Users.Domain;

namespace BodeAbp.Zero.Application.Users.Dtos
{
    [AutoMapTo(typeof(User))]
    public class CreateUserInput : IInputDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(User.MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        [StringLength(User.MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(User.MaxPhoneNoLength)]
        public string PhoneNo { get; set; }

        /// <summary>
        /// 邮箱
        /// Email address must be unique for it's tenant.
        /// </summary>
        [StringLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 注册验证码
        /// </summary>
        public ValidateCodeInput ValidateCode { get; set; }
    }
}
