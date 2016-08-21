using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Localization;
using BodeAbp.Zero.Settings.Domain;

namespace BodeAbp.Zero.Settings.Dtos
{
    /// <summary>
    /// 设置Dto
    /// </summary>
    [AutoMapFrom(typeof(Setting))]
    public class SettingDto: SettingInfo
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; set; }

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
}
