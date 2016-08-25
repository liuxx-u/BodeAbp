using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.Users.Domain;

namespace BodeAbp.Zero.Application.Users.Dtos
{
    public class ResetPasswordInput : IInputDto
    {
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(User.MaxPlainPasswordLength)]
        public string NewPassword { get; set; }
        
        /// <summary>
        /// 重置密码验证码
        /// </summary>
        public ValidateCodeInput ValidateCode { get; set; }
    }
}
