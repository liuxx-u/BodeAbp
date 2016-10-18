using Abp.Application.Services;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using BodeAbp.Product.Attributes.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;

namespace BodeAbp.Product.Attributes
{
    /// <summary>
    ///  商品属性 服务
    /// </summary>
    [Description("商品属性接口")]
    public interface IAttributesAppService : IApplicationService
    {
        #region 属性模版

        /// <summary>
        /// 获取 属性模版分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AttributeDto>> GetAttributePagedList(QueryListPagedRequestInput input);
        
        /// <summary>
        /// 添加 属性模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAttribute(AttributeDto input);

        /// <summary>
        /// 更新 属性模版
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAttribute(AttributeDto input);


        /// <summary>
        /// 删除 属性模版
        /// </summary>
        /// <param name="input">id集合</param>
        /// <returns></returns>
        Task DeleteAttribute(IdInput input);

        /// <summary>
        /// 获取可选的属性模板树
        /// </summary>
        /// <returns></returns>
        Task<ICollection<TreeOutPut>> GetOptionalAttributeTreeData();

        /// <summary>
        /// 根据分类Id获取属性分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperableAttributeGroupDto[]> GetClassifyGroupAttributes(IdInput input);

        #endregion

        #region 属性选项

        /// <summary>
        /// 获取 属性值分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AttributeOptionDto>> GetAttributeOptionPagedList(QueryListPagedRequestInput input);
        
        /// <summary>
        /// 添加 属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAttributeOption(AttributeOptionDto input);

        /// <summary>
        /// 更新 属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAttributeOption(AttributeOptionDto input);


        /// <summary>
        /// 删除 属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAttributeOption(IdInput[] input);

        #endregion

        #region 分类

        /// <summary>
        /// 获取 全部分类
        /// </summary>
        /// <returns></returns>
        Task<ICollection<ProductClassifyListOutPut>> GetAllClassifies();

        /// <summary>
        /// 获取分类树数据
        /// </summary>
        /// <returns></returns>
        Task<TreeOutPut[]> GetClassifyTreeData();

        /// <summary>
        /// 获取分类下拉选择框数据
        /// </summary>
        /// <returns></returns>
        Task<ICollection<NameValue>> GetClassifySelectedOptions();

        /// <summary>
        /// 添加 分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateClassify(ProductClassifyInput input);

        /// <summary>
        /// 更新 分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateClassify(ProductClassifyInput input);


        /// <summary>
        /// 删除 分类
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task DeleteClassify(IdInput input);

        /// <summary>
        /// 分类升序
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ClassifyUp(IdInput input);

        /// <summary>
        /// 分类降序
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ClassifyDown(IdInput input);

        #endregion
    }
}
