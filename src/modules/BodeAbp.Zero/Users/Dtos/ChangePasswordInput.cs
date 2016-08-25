using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.Users.Domain;

namespace BodeAbp.Zero.Application.Users.Dtos
{
    public class ChangePasswordInput: IInputDto
    {
        /// <summary>
        /// 原密码
        /// </summary>
        [Required]
        [StringLength(User.MaxPlainPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(User.MaxPlainPasswordLength)]
        public string NewPassword { get; set; }
    }
}
