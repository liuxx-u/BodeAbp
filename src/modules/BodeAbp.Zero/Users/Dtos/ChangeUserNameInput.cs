using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.Users.Domain;

namespace BodeAbp.Zero.Application.Users.Dtos
{
    public class ChangeUserNameInput : IInputDto
    {
        /// <summary>
        /// 新用户名
        /// </summary>
        [Required]
        [StringLength(User.MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(User.MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 修改用户名验证码
        /// </summary>
        public ValidateCodeInput ValidateCode { get; set; }
    }
}
