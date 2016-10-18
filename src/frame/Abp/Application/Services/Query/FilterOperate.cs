﻿
namespace Abp.Application.Services.Query
{
    /// <summary>
    /// 筛选操作方式
    /// </summary>
    public enum FilterOperate
    {
        /// <summary>
        /// 并且
        /// </summary>
        [OperateCode("and")]
        And = 1,

        /// <summary>
        /// 或者
        /// </summary>
        [OperateCode("or")]
        Or = 2,

        /// <summary>
        /// 等于
        /// </summary>
        [OperateCode("equal")]
        Equal = 3,

        /// <summary>
        /// 不等于
        /// </summary>
        [OperateCode("notequal")]
        NotEqual = 4,

        /// <summary>
        /// 小于
        /// </summary>
        [OperateCode("less")]
        Less = 5,

        /// <summary>
        /// 小于或等于
        /// </summary>
        [OperateCode("lessorequal")]
        LessOrEqual = 6,

        /// <summary>
        /// 大于
        /// </summary>
        [OperateCode("greater")]
        Greater = 7,

        /// <summary>
        /// 大于或等于
        /// </summary>
        [OperateCode("greaterorequal")]
        GreaterOrEqual = 8,

        /// <summary>
        /// 以……开始
        /// </summary>
        [OperateCode("startswith")]
        StartsWith = 9,

        /// <summary>
        /// 以……结束
        /// </summary>
        [OperateCode("endswith")]
        EndsWith = 10,

        /// <summary>
        /// 包含（相似）
        /// </summary>
        [OperateCode("contains")]
        Contains = 11,

        ///// <summary>
        ///// 包括在
        ///// </summary>
        //[OperateCode("in")]
        //In = 12,

        ///// <summary>
        ///// 不包括在
        ///// </summary>
        //[OperateCode("notin")]
        //NotIn = 13
    }
}