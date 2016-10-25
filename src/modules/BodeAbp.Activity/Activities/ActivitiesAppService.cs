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
using System;

namespace BodeAbp.Activity.Activities
{
	/// <summary>
    ///  活动 服务
    /// </summary>
    public class ActivitiesAppService : AsyncCrudAppService<Classify,ClassifyDto,int>, IActivitiesAppService
    {
        private readonly IRepository<Domain.Activity,long> _activityRepository;
        public ActivitiesAppService(IRepository<Domain.Activity, long> activityRepository, IRepository<Classify> classifyRepository) : base(classifyRepository)
        {
            _activityRepository = activityRepository;
        }

        #region 活动

        /// <inheritdoc/>
        public async Task<PagedResultDto<GetActivityListOutput>> GetActivityPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _activityRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultDto<GetActivityListOutput>(total, list.MapTo<List<GetActivityListOutput>>());
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
        public async Task DeleteActivity(long id)
        {
            await _activityRepository.DeleteAsync(id);
        }
        #endregion
    }
}