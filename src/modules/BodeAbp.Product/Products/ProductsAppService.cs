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
using System;
using Abp.Extensions;
using BodeAbp.Product.Attributes.Domain;
using Abp.UI;
using Abp.Domain.Entities.Asset;
using Abp.Dependency;
using BodeAbp.Product.Attributes;
using Abp.Runtime.Validation;
using BodeAbp.Product.Products.Domain;

namespace BodeAbp.Product.Products
{
    /// <summary>
    /// 商品  应用服务
    /// </summary>
    public class ProductsAppService : ApplicationService, IProductsAppService
    {
        private readonly IRepository<ProductClassify> _classifyRepository;
        private readonly IRepository<Domain.Product, long> _productRepository;
        private readonly IRepository<ProductAttributeMap, long> _productAttributeRepository;
        private readonly IRepository<ProductAsset, long> _assetRepository;
        private readonly IRepository<ProductExtendService, long> _extendServiceRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="classifyRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="productAttributeRepository"></param>
        /// <param name="assetRepository"></param>
        /// <param name="extendServiceRepository"></param>
        public ProductsAppService(
            IRepository<ProductClassify> classifyRepository,
            IRepository<Domain.Product, long> productRepository,
            IRepository<ProductAttributeMap, long> productAttributeRepository,
            IRepository<ProductAsset, long> assetRepository,
            IRepository<ProductExtendService, long> extendServiceRepository)
        {
            _classifyRepository = classifyRepository;
            _productRepository = productRepository;
            _productAttributeRepository = productAttributeRepository;
            _assetRepository = assetRepository;
            _extendServiceRepository = extendServiceRepository;
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
        public async Task<OperableProductDto> GetOperableProduct(long id)
        {
            var product = await _productRepository.GetAsync(id);
            var dto = product.MapTo<OperableProductDto>();

            dto.Albums = product.Assets.Select(p => p.Path).ToList();

            var attributeService = IocManager.Instance.Resolve<IAttributesAppService>();
            dto.GroupAttributes = await attributeService.GetClassifyGroupAttributes(new IdInput { Id = product.ClassifyId });

            foreach (var group in dto.GroupAttributes)
            {
                foreach (var attribute in group.Attributes)
                {
                    var productAttribute = product.Attributes.SingleOrDefault(p => p.AttributeId == attribute.Id);
                    if (productAttribute == null) continue;

                    attribute.Value = productAttribute.Value;
                    if (attribute.AttributeType == ProductAttributeType.Switch)
                    {
                        attribute.attributeOptionId = productAttribute.AttributeOptionIds;
                    }
                    if (attribute.AttributeType == ProductAttributeType.Multiple)
                    {
                        attribute.attributeOptionIds = productAttribute.AttributeOptionIds.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    }
                }
            }
            return dto;
        }

        /// <inheritdoc/>
        public async Task CreateProduct(OperableProductDto input)
        {
            input.CheckNotNull("input");
            input.ClassifyId.CheckGreaterThan("input.ClassifyId", 0);
            if (!_classifyRepository.CheckExists(p => p.Id == input.ClassifyId))
            {
                throw new UserFriendlyException("指定的分类不存在");
            }

            var product = input.MapTo<Domain.Product>();

            if (input.IsOnShelf)
            {
                product.OnShelfTime = DateTime.Now;
            }
            foreach (var group in input.GroupAttributes)
            {
                foreach (var item in group.Attributes)
                {
                    product.Attributes.Add(new ProductAttributeMap
                    {
                        AttributeId = item.Id,
                        Value = item.Value,
                        AttributeOptionIds = item.AttributeType == ProductAttributeType.Switch
                        ? FormatOptionIds(item.attributeOptionId)
                        : item.AttributeType == ProductAttributeType.Multiple ? FormatOptionIds(item.attributeOptionIds.ExpandAndToString()) : ""
                    });
                }
            }
            product.Assets = input.Albums.Select(p => new ProductAsset
            {
                Path = p,
                AssetType = AssetType.Picture
            }).ToList();

            await _productRepository.InsertAsync(product);
        }

        /// <inheritdoc/>
        public async Task UpdateProduct(OperableProductDto input)
        {
            input.CheckNotNull("input");
            input.Id.CheckGreaterThan("input.Id", 0);
            var product = await _productRepository.GetAsync(input.Id);

            int onShelfState = 0;//0:未更改;1:上架;2:下架;
            if (product.IsOnShelf != input.IsOnShelf) onShelfState = input.IsOnShelf ? 1 : 2;

            input.MapTo(product);
            if (onShelfState == 1) product.OnShelfTime = DateTime.Now;
            if (onShelfState == 2) product.OffShelfTime = DateTime.Now;

            //更新属性
            var exsitAttributes = await _productAttributeRepository.GetAll().Where(p => p.ProductId == input.Id).ToListAsync();
            foreach (var group in input.GroupAttributes)
            {
                foreach (var item in group.Attributes)
                {
                    var attribute = new ProductAttributeMap
                    {
                        ProductId = input.Id,
                        AttributeId = item.Id,
                        Value = item.Value,
                        AttributeOptionIds = item.AttributeType == ProductAttributeType.Switch
                        ? FormatOptionIds(item.attributeOptionId)
                        : item.AttributeType == ProductAttributeType.Multiple ? FormatOptionIds(item.attributeOptionIds.ExpandAndToString()) : ""
                    };

                    var exsitAttribute = exsitAttributes.SingleOrDefault(p => p.AttributeId == attribute.AttributeId);
                    if (exsitAttribute == null)
                    {
                        await _productAttributeRepository.InsertAsync(attribute);
                    }
                    else
                    {
                        if (attribute.Value != exsitAttribute.Value || attribute.AttributeOptionIds != exsitAttribute.AttributeOptionIds)
                        {
                            exsitAttribute.Value = attribute.Value;
                            exsitAttribute.AttributeOptionIds = attribute.AttributeOptionIds;
                            await _productAttributeRepository.UpdateAsync(exsitAttribute);
                        }
                    }
                }
            }

            //更新相册
            string[] existPaths = product.Assets.Select(m => m.Path).ToArray();
            string[] addPaths = input.Albums.Except(existPaths).ToArray();
            string[] removePaths = existPaths.Except(input.Albums).ToArray();
            foreach (var path in addPaths)
            {
                await _assetRepository.InsertAsync(new ProductAsset
                {
                    Path = path,
                    ProductId = input.Id,
                    AssetType = AssetType.Picture
                });
            }
            await _assetRepository.DeleteAsync(p => removePaths.Contains(p.Path));
            //更新商品
            await _productRepository.UpdateAsync(product);
        }

        /// <inheritdoc/>
        public async Task DeleteProduct(IdInput input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }

        //将OptionIds格式化为,1,2,3,方便查询
        private string FormatOptionIds(string strIds)
        {
            if (strIds.IsNullOrWhiteSpace()) return string.Empty;
            return string.Format(",{0},", strIds);
        }

        #endregion

        #region 增值服务

        /// <inheritdoc/>
        public async Task<PagedResultDto<ProductExtendServiceDto>> GetExtendServicePagedList(QueryListPagedRequestInput input)
        {
            //验证商品Id的查询参数
            if (input.FilterGroup == null || !input.FilterGroup.Rules.Any(p => p.Field == "productId"))
            {
                throw new AbpValidationException("查询参数错误"); 
            }

            int total;
            var list = await _extendServiceRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultDto<ProductExtendServiceDto>(total, list.MapTo<List<ProductExtendServiceDto>>());
        }

        /// <inheritdoc/>
        public async Task CreateExtendService(ProductExtendServiceDto input)
        {
            if (_extendServiceRepository.CheckExists(p => p.ProductId == input.ProductId && p.Name == input.Name))
            {
                throw new AbpValidationException("该增值服务已存在");
            }
            var extendService = input.MapTo<ProductExtendService>();
            await _extendServiceRepository.InsertAsync(extendService);
        }

        /// <inheritdoc/>
        public async Task UpdateExtendService(ProductExtendServiceDto input)
        {
            input.Id.CheckGreaterThan("input.Id", 0);
            if (_extendServiceRepository.CheckExists(p => p.ProductId == input.ProductId && p.Name == input.Name,input.Id))
            {
                throw new AbpValidationException("该增值服务已存在");
            }
            var extendService = await _extendServiceRepository.GetAsync(input.Id);
            input.MapTo(extendService);
            await _extendServiceRepository.UpdateAsync(extendService);
        }

        /// <inheritdoc/>
        public async Task DeleteExtendService(IdInput<long> input)
        {
            await _extendServiceRepository.DeleteAsync(input.Id);
        }

        #endregion
    }
}
