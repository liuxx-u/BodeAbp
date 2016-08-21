using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Zero.Settings.Domain
{
    /// <summary>
    /// Represents a setting for a user.
    /// </summary>
    [Table("Zero#Setting")]
    public class Setting : AuditedEntity<long>
    {
        #region 常量

        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 256;

        /// <summary>
        /// Maximum length of the <see cref="Value"/> property.
        /// </summary>
        public const int MaxValueLength = 2000;

        #endregion

        #region 属性

        /// <summary>
        /// UserId for this setting.
        /// UserId is null if this setting is application level.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Value of the setting.
        /// </summary>
        [MaxLength(MaxValueLength)]
        public string Value { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// Creates a new <see cref="Setting"/> object.
        /// </summary>
        public Setting()
        {

        }

        /// <summary>
        /// Creates a new <see cref="Setting"/> object.
        /// </summary>
        /// <param name="userId">UserId for this setting</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public Setting(long? userId, string name, string value)
        {
            UserId = userId;
            Name = name;
            Value = value;
        }

        #endregion
    }
}