using System;
using Abp.AutoMapper;
using BodeAbp.Zero.Users.Domain;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BodeAbp.Zero.Users.Dtos
{
    public abstract class UserDto : EntityDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(0)]
        public string UserName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// 邮箱Emailaddressmustbeuniqueforit'stenant.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 是否验证邮箱.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// 是否验证手机
        /// </summary>
        public bool IsPhoneNoConfirm { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

    }

    [AutoMapFrom(typeof(User))]
    public class GetUserListOutput : UserDto, IOutputDto
    {
    }
}


