using System.ComponentModel;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using BodeAbp.Zero.Settings.Dtos;

namespace BodeAbp.Zero.Settings
{
    /// <summary>
    /// 设置信息服务
    /// </summary>
    [Description("设置信息接口")]
    public interface ISettingsAppService : IApplicationService
    {
        /// <summary>
        /// 获取 系统设置 分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<SettingDto>> GetApplicationSettingPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 修改设置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeSetting(SettingDto input);
    }
}
