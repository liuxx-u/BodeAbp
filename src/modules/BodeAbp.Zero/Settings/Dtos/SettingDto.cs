using System.Collections.Generic;
using Abp.AutoMapper;
using Abp.Configuration;
using BodeAbp.Zero.Settings.Domain;

namespace BodeAbp.Zero.Settings.Dtos
{
    /// <summary>
    /// 设置Dto
    /// </summary>
    [AutoMap(typeof(Setting))]
    public class SettingDto: SettingInfo
    {
        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Display name of the setting.
        /// This can be used to show setting to the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A brief description for this setting.
        /// </summary>
        public string Description { get; set; }
    }

    public class SettingGroup
    {
        public SettingGroup()
        {
            Settings = new List<SettingDto>();
        }

        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 设置项集合
        /// </summary>
        public ICollection<SettingDto> Settings { get; set; }
    }
}
