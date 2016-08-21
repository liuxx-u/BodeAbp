using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.UI;
using BodeAbp.Zero.Settings.Domain;
using BodeAbp.Zero.Settings.Dtos;

namespace BodeAbp.Zero.Settings
{
    /// <summary>
    /// 设置信息 服务
    /// </summary>
    public class SettingsAppService : ApplicationService, ISettingsAppService
    {
        /// <summary>
        /// 设置项管理器
        /// </summary>
        public ISettingDefinitionManager SettingDefinitionManager { protected get; set; }

        /// <summary>
        /// 设置信息仓储
        /// </summary>
        public IRepository<Setting, long> SettingRepository { protected get; set; }
        
        private readonly ITypedCache<string, Dictionary<string, SettingInfo>> _applicationSettingCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SettingsAppService(ICacheManager cacheManager)
        {
            _applicationSettingCache = cacheManager.GetApplicationSettingsCache();
        }

        /// <inheritdoc/>
        public async Task<PagedResultOutput<SettingDto>> GetApplicationSettingPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var settings = (await SettingManager.GetAllSettingValuesAsync(SettingScopes.Application)).Select(p => new Setting(null, p.Name, p.Value));
            var result = settings.AsQueryable().Where(input, out total);
            var dtos = new List<SettingDto>();
            foreach (var item in result)
            {
                var definition = SettingDefinitionManager.GetSettingDefinition(item.Name);
                dtos.Add(new SettingDto()
                {
                    Id = item.Name,
                    Name = item.Name,
                    Value = item.Value,
                    DisplayName = definition.DisplayName == null ? "" : definition.DisplayName.Localize(localizationContext),
                    Description = definition.Description == null ? "" : definition.Description.Localize(localizationContext),
                });
            }

            return new PagedResultOutput<SettingDto>(total, dtos);
        }

        /// <inheritdoc/>
        public async Task ChangeSetting(SettingDto input)
        {
            var setting = SettingRepository.GetAll().FirstOrDefault(p => p.Name == input.Name);
            if (setting == null)
            {
                throw new UserFriendlyException("指定的设置项不存在");
            }
            setting.Name = input.Name;
            setting.Value = input.Value;
            setting.Description = input.Description;
            setting.DisplayName = input.DisplayName;
            await SettingRepository.UpdateAsync(setting);
            _applicationSettingCache.Clear();
        }
    }
}
