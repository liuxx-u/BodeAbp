using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BodeAbp.Product.Skus.Dtos;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BodeAbp.Product.Skus
{
    /// <summary>
    /// SKU应用服务
    /// </summary>
    [Description("SKU接口")]
    public interface ISkusAppService : IApplicationService
    {
        #region SKU属性模板

        /// <summary>
        /// 获取 SKU属性模版分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<SkuAttributeDto>> GetSkuAttributePagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 添加 SKU属性模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateSkuAttribute(SkuAttributeDto input);

        /// <summary>
        /// 更新 SKU属性模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateSkuAttribute(SkuAttributeDto input);


        /// <summary>
        /// 删除 SKU属性模版
        /// </summary>
        /// <param name="input">id</param>
        /// <returns></returns>
        Task DeleteSkuAttribute(IdInput input);

        /// <summary>
        /// 获取 SKU属性树数据
        /// </summary>
        /// <returns></returns>
        Task<ICollection<TreeOutPut>> GetOptionalSkuAttributeTreeData();

        #endregion

        #region SKU属性选项

        /// <summary>
        /// 获取 SKU属性选项分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<SkuAttributeOptionDto>> GetSkuAttributeOptionPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 新增 SKU属性选项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateSkuAttributeOption(SkuAttributeOptionDto input);

        /// <summary>
        /// 更新 SKU属性选项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateSkuAttributeOption(SkuAttributeOptionDto input);

        /// <summary>
        /// 删除SKU属性选项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteSkuAttributeOption(IdInput[] input);

        #endregion
    }
}
