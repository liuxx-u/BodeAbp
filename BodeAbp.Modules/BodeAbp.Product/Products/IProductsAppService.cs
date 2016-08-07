using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BodeAbp.Product.Products.Dtos;

namespace BodeAbp.Product.Products
{
    /// <summary>
    /// 商品  应用服务
    /// </summary>
    [Description("商品接口")]
    public interface IProductsAppService : IApplicationService
    {
        #region 商品

        /// <summary>
        /// 获取 商品分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetProductListOutput>> GetProductPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取 商品详情
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<GetProductOutput> GetProduct(int id);

        /// <summary>
        /// 添加 商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateProduct(CreateProductInput input);

        /// <summary>
        /// 更新 商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProduct(UpdateProductInput input);


        /// <summary>
        /// 删除 商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteProduct(List<IdInput> input);

        #endregion
    }
}
