using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace BodeAbp.Activity.Activities.Domain
{
    /// <summary>
    /// 活动 领域服务
    /// </summary>
    public class ActivityManager : IDomainService
    {
        /// <summary>
        /// 活动  仓储
        /// </summary>
        public IRepository<Activity, long> activityRepository { protected get; set; }
        
        /// <summary>
        /// 分类  仓储
        /// </summary>
        public IRepository<Classify, int> classifyRepository { protected get; set; }

        /// <summary>
        /// 创建  活动
        /// </summary>
        /// <param name="activity">活动</param>
        /// <returns></returns>
        public async Task CreateActivityAsync(Activity activity)
        {
            await activityRepository.InsertAsync(activity);
        }

        /// <summary>
        /// 更新  活动
        /// </summary>
        /// <param name="activity">活动</param>
        /// <returns></returns>
        public async Task UpdateActivityAsync(Activity activity)
        {
            await activityRepository.UpdateAsync(activity);
        }

        /// <summary>
        /// 删除 活动
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public async Task DeleteActivityAsync(long activityId)
        {
            await activityRepository.DeleteAsync(activityId);
        }

        /// <summary>
        /// 获取活动详情
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public async Task<Activity> GetActivity(long activityId)
        {
            var activity = await activityRepository.GetAsync(activityId);
            await BrowseActivity(activity);
            return activity;
        }

        private async Task BrowseActivity(Activity activity)
        {
            activity.BrowseNo++;
            await activityRepository.UpdateAsync(activity);
        }
    }
}
