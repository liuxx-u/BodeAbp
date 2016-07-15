using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Configuration;

namespace BodeAbp.Zero.Settings
{
    /// <summary>
    /// 设置信息 服务
    /// </summary>
    public class SettingsAppService : ApplicationService, ISettingsAppService
    {
        /// <inheritdoc/>
        public Task<PagedResultOutput<SettingInfo>> GetApplicationSettingPagedList(QueryListPagedRequestInput input)
        {
            throw new NotImplementedException();
        }
    }
}
