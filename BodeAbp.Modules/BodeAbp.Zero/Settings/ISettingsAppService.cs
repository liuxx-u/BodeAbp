using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
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
        /// 获取 系统设置 分组
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SettingGroup>> GetApplicationSettingGroups();

        /// <summary>
        /// 修改设置组信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeGroupSetting(SettingGroup input);
    }
}
