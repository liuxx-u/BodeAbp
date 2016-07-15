using Abp.Application.Services;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using BodeAbp.Product.Attributes.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BodeAbp.Product.Attributes
{
	/// <summary>
    ///  属性 服务
    /// </summary>
	[Description("属性接口")]
    public interface IAttributesAppService : IApplicationService
    {
        #region 属性模版

        /// <summary>
        /// 获取 属性模版分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetAttributeListOutput>> GetAttributePagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取 属性模版详情
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<GetAttributeOutput> GetAttribute(int id);

        /// <summary>
        /// 添加 属性模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAttribute(CreateAttributeInput input);

        /// <summary>
        /// 更新 属性模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAttribute(UpdateAttributeInput input);


        /// <summary>
        /// 删除 属性模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAttribute(List<IdInput> input);

        #endregion

        #region 属性值

        /// <summary>
        /// 获取 属性值分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetAttributeOptionListOutput>> GetAttributeOptionPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取 属性值详情
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<GetAttributeOptionOutput> GetAttributeOption(int id);

        /// <summary>
        /// 添加 属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAttributeOption(CreateAttributeOptionInput input);

        /// <summary>
        /// 更新 属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAttributeOption(UpdateAttributeOptionInput input);


        /// <summary>
        /// 删除 属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAttributeOption(List<IdInput> input);

        #endregion

        #region 分类
        
        /// <summary>
        /// 获取 分类分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetProductClassifyListOutput>> GetClassifyPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取 分类详情
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<GetProductClassifyOutput> GetClassify(int id);

        /// <summary>
        /// 添加 分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateClassify(CreateProductClassifyInput input);

        /// <summary>
        /// 更新 分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateClassify(UpdateProductClassifyInput input);


        /// <summary>
        /// 删除 分类
        /// </summary>
        /// <param name="classifyId">分类Id</param>
        /// <returns></returns>
        Task DeleteClassify(int classifyId);

        #endregion
    }
}
