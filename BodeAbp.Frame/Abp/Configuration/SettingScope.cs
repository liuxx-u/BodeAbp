using System;

namespace Abp.Configuration
{
    /// <summary>
    /// Defines scope of a setting.
    /// </summary>
    [Flags]
    public enum SettingScopes
    {
        /// <summary>
        /// Represents a setting that can be configured/changed for the application level.
        /// </summary>
        Application = 1,

        /// <summary>
        /// Represents a setting that can be configured/changed for each User.
        /// </summary>
        User = 2
    }
}