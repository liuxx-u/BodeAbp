using System.Collections.Generic;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// 树 OutputDto
    /// </summary>
    public class TreeOutPut : IOutputDto
    {
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public ICollection<TreeOutPut> Children { get; set; }
    }
}
