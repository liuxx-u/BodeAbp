using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Extensions;

namespace BodeAbp.Zero.Auditing.Domain
{
    /// <summary>
    /// 审计日志
    /// </summary>
    [Table("Zero#AuditLog")]
    public class AuditLog : Entity<long>
    {
        /// <summary>
        /// Maximum length of <see cref="ServiceName"/> property.
        /// </summary>
        public const int MaxServiceNameLength = 256;

        /// <summary>
        /// Maximum length of <see cref="MethodName"/> property.
        /// </summary>
        public const int MaxMethodNameLength = 256;

        /// <summary>
        /// Maximum length of <see cref="Parameters"/> property.
        /// </summary>
        public const int MaxParametersLength = 1024;

        /// <summary>
        /// Maximum length of <see cref="ClientIpAddress"/> property.
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="ClientName"/> property.
        /// </summary>
        public const int MaxClientNameLength = 128;

        /// <summary>
        /// Maximum length of <see cref="BrowserInfo"/> property.
        /// </summary>
        public const int MaxBrowserInfoLength = 256;

        /// <summary>
        /// Maximum length of <see cref="Exception"/> property.
        /// </summary>
        public const int MaxExceptionLength = 2000;

        /// <summary>
        /// Maximum length of <see cref="CustomData"/> property.
        /// </summary>
        public const int MaxCustomDataLength = 2000;
        
        /// <summary>
        /// UserId.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        [MaxLength(MaxServiceNameLength)]
        public virtual string ServiceName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        [MaxLength(MaxMethodNameLength)]
        public virtual string MethodName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [MaxLength(MaxParametersLength)]
        public virtual string Parameters { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public virtual DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public virtual int ExecutionDuration { get; set; }

        /// <summary>
        /// 客户端ip
        /// </summary>
        [MaxLength(MaxClientIpAddressLength)]
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        [MaxLength(MaxClientNameLength)]
        public virtual string ClientName { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        [MaxLength(MaxBrowserInfoLength)]
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [MaxLength(MaxExceptionLength)]
        public virtual string Exception { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual long? ImpersonatorUserId { get; set; }
        
        /// <summary>
        /// 自定义数据
        /// </summary>
        [MaxLength(MaxCustomDataLength)]
        public virtual string CustomData { get; set; }

        /// <summary>
        /// Creates a new CreateFromAuditInfo from given <see cref="AuditInfo"/>.
        /// </summary>
        /// <param name="auditInfo">Source <see cref="AuditInfo"/> object</param>
        /// <returns>The <see cref="AuditLog"/> object that is created using <see cref="AuditInfo"/></returns>
        public static AuditLog CreateFromAuditInfo(AuditInfo auditInfo)
        {
            var exceptionMessage = auditInfo.Exception != null ? auditInfo.Exception.ToString() : null;
            return new AuditLog
                   {
                       UserId = auditInfo.UserId,
                       ServiceName = auditInfo.ServiceName.TruncateWithPostfix(MaxServiceNameLength),
                       MethodName = auditInfo.MethodName.TruncateWithPostfix(MaxMethodNameLength),
                       Parameters = auditInfo.Parameters.TruncateWithPostfix(MaxParametersLength),
                       ExecutionTime = auditInfo.ExecutionTime,
                       ExecutionDuration = auditInfo.ExecutionDuration,
                       ClientIpAddress = auditInfo.ClientIpAddress.TruncateWithPostfix(MaxClientIpAddressLength),
                       ClientName = auditInfo.ClientName.TruncateWithPostfix(MaxClientNameLength),
                       BrowserInfo = auditInfo.BrowserInfo.TruncateWithPostfix(MaxBrowserInfoLength),
                       Exception = exceptionMessage.TruncateWithPostfix(MaxExceptionLength),
                       ImpersonatorUserId = auditInfo.ImpersonatorUserId,
                       CustomData = auditInfo.CustomData.TruncateWithPostfix(MaxCustomDataLength)
                   };
        }

        /// <summary>
        /// 重载ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                "AUDIT LOG: {0}.{1} is executed by user {2} in {3} ms from {4} IP address.",
                ServiceName, MethodName, UserId, ExecutionDuration, ClientIpAddress
                );
        }
    }
}
