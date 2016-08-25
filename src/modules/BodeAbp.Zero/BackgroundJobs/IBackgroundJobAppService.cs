using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.BackgroundJobs.Dtos;

namespace BodeAbp.Zero.BackgroundJobs
{
    /// <summary>
    /// 后台任务 应用服务
    /// </summary>
    [Description("后台任务接口")]
    public interface IBackgroundJobAppService : IApplicationService
    {
        /// <summary>
        /// 获取 后台任务 分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetBackgroundJobListOutput>> GetBackgroundJobPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 删除 审计日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteBackgroundJob(List<IdInput<long>> input);
    }
}
