using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
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
        public ISettingDefinitionManager _settingDefinitionManager { protected get; set; }

        /// <summary>
        /// 设置信息仓储
        /// </summary>
        public IRepository<Setting, long> _settingRepository { protected get; set; }

        /// <inheritdoc/>
        public async Task<IEnumerable<SettingGroup>> GetApplicationSettingGroups()
        {
            var settings = (await SettingManager.GetAllSettingValuesAsync(SettingScopes.Application)).Select(p => new Setting(null, p.Name, p.Value));
            var dtos = new List<SettingDto>();
            foreach (var item in settings)
            {
                var definition = _settingDefinitionManager.GetSettingDefinition(item.Name);
                dtos.Add(new SettingDto()
                {
                    Name = item.Name,
                    Value = item.Value,
                    GroupName = definition.Group == null ? "系统设置" : definition.Group.DisplayName.Localize(localizationContext),
                    DisplayName = definition.DisplayName == null ? "" : definition.DisplayName.Localize(localizationContext),
                    Description = definition.Description == null ? "" : definition.Description.Localize(localizationContext),
                });
            }

            return dtos.GroupBy(p => p.GroupName).Select(p => new SettingGroup()
            {
                GroupName = p.Key,
                Settings = p.ToList()
            });
        }

        /// <inheritdoc/>
        public async Task ChangeGroupSetting(SettingGroup input)
        {
            input.Settings.CheckNotNull("input.Settings");
            input.GroupName.CheckNotNull("input.GroupName");

            var groupSettings = _settingRepository.GetAll().Where(p => p.GroupName == input.GroupName);

            foreach (var dto in input.Settings)
            {
                if (groupSettings.Any(p => p.Name == dto.Name))
                {
                    var setting = groupSettings.Single(p => p.Name == dto.Name);
                    setting.Value = dto.Value;
                    await _settingRepository.UpdateAsync(setting);
                }
                else
                {
                    var setting = dto.MapTo<Setting>();
                    await _settingRepository.InsertAsync(setting);
                }
            }
        }
    }
}
