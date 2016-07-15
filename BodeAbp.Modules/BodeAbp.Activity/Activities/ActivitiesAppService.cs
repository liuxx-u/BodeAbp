using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BodeAbp.Activity.Activities.Domain;
using BodeAbp.Activity.Activities.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BodeAbp.Activity.Activities
{
	/// <summary>
    ///  活动 服务
    /// </summary>
    public class ActivitiesAppService : IActivitiesAppService
    {
        private readonly IRepository<Classify> _classifyRepository;
        private readonly IRepository<Domain.Activity,long> _activityRepository;
        public ActivitiesAppService(IRepository<Domain.Activity,long> activityRepository, IRepository<Classify> classifyRepository)
        {
            _classifyRepository = classifyRepository;
            _activityRepository = activityRepository;
        }

        #region 活动

        /// <inheritdoc/>
        public async Task<PagedResultOutput<GetActivityListOutput>> GetActivityPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _activityRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetActivityListOutput>(total, list.MapTo<List<GetActivityListOutput>>());
        }

        /// <inheritdoc/>
        public async Task<GetActivityOutput> GetActivity(IdInput input)
        {
            var result = await _activityRepository.GetAsync(input.Id);
            return result.MapTo<GetActivityOutput>();
        }

        /// <inheritdoc/>
        public async Task CreateActivity(CreateActivityInput input)
        {
            var activity = input.MapTo<Domain.Activity>();
            await _activityRepository.InsertAsync(activity);
        }

        /// <inheritdoc/>
        public async Task UpdateActivity(UpdateActivityInput input)
        {
            var activity = await _activityRepository.GetAsync(input.Id);
            input.MapTo(activity);
            await _activityRepository.UpdateAsync(activity);
        }

        /// <inheritdoc/>
        public async Task DeleteActivity(List<IdInput<long>> input)
        {
            var ids = input.Select(p => p.Id);
            await _activityRepository.DeleteAsync(p => ids.Contains(p.Id));
        }
        #endregion

        #region 活动类型

        /// <inheritdoc/>
        public async Task<PagedResultOutput<GetClassifyListOutput>> GetClassifyPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _classifyRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetClassifyListOutput>(total, list.MapTo<List<GetClassifyListOutput>>());
        }

        /// <inheritdoc/>
        public async Task<GetClassifyOutput> GetClassify(IdInput input)
        {
            var result = await _classifyRepository.GetAsync(input.Id);
            return result.MapTo<GetClassifyOutput>();
        }

        /// <inheritdoc/>
        public async Task CreateClassify(CreateClassifyInput input)
        {
            var classify = input.MapTo<Classify>();
            await _classifyRepository.InsertAsync(classify);
        }

        /// <inheritdoc/>
        public async Task UpdateClassify(UpdateClassifyInput input)
        {
            var classify = await _classifyRepository.GetAsync(input.Id);
            input.MapTo(classify);
            await _classifyRepository.UpdateAsync(classify);
        }

        /// <inheritdoc/>
        public async Task DeleteClassify(List<IdInput> input)
        {
            var ids = input.Select(p => p.Id);
            await _classifyRepository.DeleteAsync(p => ids.Contains(p.Id));
        }

        #endregion
    }
}