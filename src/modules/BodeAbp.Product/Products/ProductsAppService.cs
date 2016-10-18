using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BodeAbp.Product.Products.Dtos;

namespace BodeAbp.Product.Products
{
    /// <summary>
    /// 商品  应用服务
    /// </summary>
    public class ProductsAppService : ApplicationService, IProductsAppService
    {
        private readonly IRepository<Domain.Product,long> _productRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductsAppService(IRepository<Domain.Product, long> productRepository)
        {
            _productRepository = productRepository;
        }

        #region 商品

        /// <inheritdoc/>
        public async Task<PagedResultDto<GetProductListOutput>> GetProductPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _productRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultDto<GetProductListOutput>(total, list.MapTo<List<GetProductListOutput>>());
        }

        /// <inheritdoc/>
        public async Task<GetProductOutput> GetProduct(int id)
        {
            var result = await _productRepository.GetAsync(id);
            return result.MapTo<GetProductOutput>();
        }

        /// <inheritdoc/>
        public async Task CreateProduct(OperableProductDto input)
        {
            var product = input.MapTo<Domain.Product>();
            await _productRepository.InsertAsync(product);
        }

        /// <inheritdoc/>
        public async Task UpdateProduct(OperableProductDto input)
        {
            var product = await _productRepository.GetAsync(input.Id);
            input.MapTo(product);
            await _productRepository.UpdateAsync(product);
        }

        /// <inheritdoc/>
        public async Task DeleteProduct(IdInput input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }

        #endregion
    }
}
