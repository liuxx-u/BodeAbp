using Abp.Application.Services.Dto;

namespace Abp.Application.Services.Query
{
    /// <summary>
    /// 表格分页查询Input扩展类
    /// </summary>
    public static class QueryListRequestInputExtensions
    {
        /// <summary>
        /// 替换查询Input中的数据字段
        /// </summary>
        /// <param name="input">dto</param>
        /// <param name="fieldName">原字段名</param>
        /// <param name="newFieldName">替换后的字段名</param>
        public static void Replace(this QueryListRequestInput input,string fieldName,string newFieldName)
        {
            foreach (var sortCondition in input.SortConditions)
            {
                if (sortCondition.SortField == fieldName)
                {
                    sortCondition.SortField = newFieldName;
                }
            }
            foreach (var rule in input.FilterGroup.Rules)
            {
                if (rule.Field == fieldName)
                {
                    rule.Field = newFieldName;
                }
            }
        }
    }
}
