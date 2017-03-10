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
        public string UserName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// 邮箱Emailaddressmustbeuniqueforit'stenant.
        /// </summary>
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

    }

    [AutoMapFrom(typeof(User))]
    public class GetUserListOutput : UserDto
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 是否验证邮箱.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// 是否验证手机
        /// </summary>
        public bool IsPhoneNoConfirm { get; set; }
    }

    [AutoMapTo(typeof(User))]
    public class CreateUserInput : UserDto
    {
        /// <summary>
        /// 密码
        /// </summary>
        //[Required]
        public string Password { get; set; }
    }

    [AutoMapTo(typeof(User))]
    public class UpdateUserInput : UserDto { }


    public class UserModifyMwd : UserDto
    {
        public string Password { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string Xpassword { get; set; }

        /// <summary>
        /// 确认密码对比
        /// </summary>
        public string Qpassword { get; set; }
    }
    #region  个人资料
    public class UserListOutPut
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// 邮箱地址
        /// Email address must be unique for it's tenant.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// CC豆
        /// </summary>
        public decimal CcCoin { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }


    public class UserInfo : UserListOutPut
    {
        /// <summary>
        /// 是否验证邮箱 
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// 是否邮箱验证
        /// </summary>
        public string IsEmailText { get; set; }
        /// <summary>
        /// 是否验证手机
        /// </summary>
        public bool IsPhoneNoConfirm { get; set; }

        /// <summary>
        /// 是否验证手机
        /// </summary>
        public string IsPhoneText { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 是否激活文字显示
        /// </summary>
        public string IsActiveText { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPic { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 性别文本显示
        /// </summary>
        public string SexText { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// 用户类型文本显示
        /// </summary>
        public string UserTypeText { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactNo { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

    }
    #endregion
}


