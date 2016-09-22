using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BodeAbp.Zero.Users.Domain;
using System;

namespace BodeAbp.Zero.Users.Dtos
{
    [AutoMapFrom(typeof(UserLoginAttempt))]
    public class UserLoginAttemptOutPut : EntityDto<long>, IOutputDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 登录客户端IP.
        /// </summary>
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// 登录客户端设备名称.
        /// </summary>
        public virtual string ClientName { get; set; }

        /// <summary>
        /// 浏览器信息.
        /// </summary>
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// 登录结果.
        /// </summary>
        public virtual AbpLoginResultType Result { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
