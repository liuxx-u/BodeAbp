using Abp.Application.Services;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using BodeAbp.Activity.Activities.Dtos;
using System.Threading.Tasks;

namespace BodeAbp.Activity.Activities
{
    /// <summary>
    ///  活动 服务
    /// </summary>
    [Description("活动接口")]
    public interface IActivitiesAppService : IApplicationService, IAsyncCrudAppService<ClassifyDto, int>
    {
        #region 活动

        /// <summary>
        /// 获取 活动分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GetActivityListOutput>> GetActivityPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取 活动详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetActivityOutput> GetActivity(IdInput input);

        /// <summary>
        /// 添加 活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateActivity(CreateActivityInput input);

        /// <summary>
        /// 更新 活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateActivity(UpdateActivityInput input);


        /// <summary>
        /// 删除 活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteActivity(long id);

        #endregion
    }
}
