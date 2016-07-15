using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 商品  领域服务
    /// </summary>
    public class ProductManager : IDomainService
    {
        private readonly IRepository<Product, long> _productRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductManager(IRepository<Product, long> productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// 创建  产品
        /// </summary>
        /// <param name="product">产品</param>
        /// <returns></returns>
        public async Task CreateProductAsync(Product product)
        {
            await _productRepository.InsertAsync(product);
        }

        /// <summary>
        /// 更新  产品
        /// </summary>
        /// <param name="product">产品</param>
        /// <returns></returns>
        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="product">产品</param>
        /// <returns></returns>
        public async Task OnShelf(Product product)
        {
            product.IsOnShelf = true;
            await _productRepository.UpdateAsync(product);
        }

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="product">产品</param>
        /// <returns></returns>
        public async Task OffShelf(Product product)
        {
            product.IsOnShelf = false;
            await _productRepository.UpdateAsync(product);
        }
    }
}
