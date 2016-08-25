using System;
using Abp.AutoMapper;
using BodeAbp.Zero.Auditing.Domain;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BodeAbp.Zero.Auditing.Dtos
{
    /// <summary>
    /// 输出列表Dto
    /// </summary>
    [AutoMapFrom(typeof(AuditLog))]
    public class GetAuditLogListOutput : EntityDto, IOutputDto
    {
        /// <summary>
        /// UserId.
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        [MaxLength(0)]
        public string ServiceName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        [MaxLength(0)]
        public string MethodName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [MaxLength(0)]
        public string Parameters { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public int ExecutionDuration { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        [MaxLength(0)]
        public string ClientIpAddress { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        [MaxLength(0)]
        public string ClientName { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        [MaxLength(0)]
        public string BrowserInfo { get; set; }

    }
}


