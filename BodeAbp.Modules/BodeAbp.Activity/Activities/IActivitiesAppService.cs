using Abp.Application.Services;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using BodeAbp.Activity.Activities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BodeAbp.Activity.Activities
{
	/// <summary>
    ///  活动 服务
    /// </summary>
	[Description("活动接口")]
    public interface IActivitiesAppService : IApplicationService
    {
        #region 活动
        
        /// <summary>
        /// 获取 活动分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetActivityListOutput>> GetActivityPagedList(QueryListPagedRequestInput input);

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
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteActivity(List<IdInput<long>> input);

        #endregion

        #region 活动类型

        /// <summary>
        /// 获取 活动类型分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetClassifyListOutput>> GetClassifyPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取 活动类型详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetClassifyOutput> GetClassify(IdInput input);

        /// <summary>
        /// 添加 活动类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateClassify(CreateClassifyInput input);

        /// <summary>
        /// 更新 活动类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateClassify(UpdateClassifyInput input);


        /// <summary>
        /// 删除 活动类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteClassify(List<IdInput> input);

        #endregion
    }
}
