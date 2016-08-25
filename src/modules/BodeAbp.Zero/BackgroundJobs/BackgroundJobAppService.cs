using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using BodeAbp.Zero.BackgroundJobs.Dtos;

namespace BodeAbp.Zero.BackgroundJobs
{
    /// <summary>
    /// 后台工作者应用服务
    /// </summary>
    public class BackgroundJobAppService : ApplicationService, IBackgroundJobAppService
    {
        private readonly IRepository<BackgroundJobInfo,long> _backgroundJobRepository;

        public BackgroundJobAppService(IRepository<BackgroundJobInfo, long> backgroundJobRepository)
        {
            _backgroundJobRepository = backgroundJobRepository;
        }

        /// <inheritdoc/>
        public async Task<PagedResultOutput<GetBackgroundJobListOutput>> GetBackgroundJobPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _backgroundJobRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetBackgroundJobListOutput>(total, list.MapTo<List<GetBackgroundJobListOutput>>());
        }

        /// <inheritdoc/>
        public async Task DeleteBackgroundJob(List<IdInput<long>> input)
        {
            var ids = input.Select(p => p.Id);
            await _backgroundJobRepository.DeleteAsync(p => ids.Contains(p.Id));
        }
    }
}
