using Abp.Application.Services;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.Auditing.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BodeAbp.Zero.Auditing
{
	/// <summary>
    ///  审计日志 服务
    /// </summary>
	[Description("审计日志接口")]
    public interface IAuditingAppService : IApplicationService
    {
        /// <summary>
        /// 获取 审计日志分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetAuditLogListOutput>> GetAuditLogPagedList(QueryListPagedRequestInput input);
        
        /// <summary>
        /// 删除 审计日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAuditLog(List<IdInput<long>> input);
    }
}
