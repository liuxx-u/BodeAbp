using System.Collections.Generic;
using Abp.Application.Services.Query;

namespace Abp.Application.Services.Dto
{
    public class QueryListRequestInput : IInputDto
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryListRequestInput()
        {
            SortConditions = new List<SortCondition>();
        }

        /// <summary>
        /// 获取或设置 查询条件组
        /// </summary>
        public FilterGroup FilterGroup { get; set; }

        /// <summary>
        /// 获取或设置 排序条件组
        /// </summary>
        public ICollection<SortCondition> SortConditions { get; set; }
    }
}
