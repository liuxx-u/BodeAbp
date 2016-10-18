using System.ComponentModel;
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
        Task<PagedResultDto<GetProductListOutput>> GetProductPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 获取 编辑的商品详情
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<OperableProductDto> GetOperableProduct(long id);

        /// <summary>
        /// 添加 商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateProduct(OperableProductDto input);

        /// <summary>
        /// 更新 商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProduct(OperableProductDto input);


        /// <summary>
        /// 删除 商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteProduct(IdInput input);

        #endregion

        #region 增值服务
        
        /// <summary>
        /// 获取 增值服务分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductExtendServiceDto>> GetExtendServicePagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 添加 增值服务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateExtendService(ProductExtendServiceDto input);

        /// <summary>
        /// 更新 增值服务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateExtendService(ProductExtendServiceDto input);


        /// <summary>
        /// 删除 增值服务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteExtendService(IdInput<long> input);

        #endregion
    }
}
