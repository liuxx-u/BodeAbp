namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// 分页查询 输入
    /// </summary>
    public class QueryListPagedRequestInput : QueryListRequestInput
    {
        /// <summary>
        /// 页序号
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
    }
}

