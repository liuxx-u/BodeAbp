using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Events.Bus.Handlers;
using Abp.Extensions;
using Abp.UI;
using BodeAbp.Product.Attributes.Domain;
using BodeAbp.Product.Skus.Event;

namespace BodeAbp.Product.Skus.Domain
{
    /// <summary>
    /// SKU  领域服务
    /// </summary>
    public class SkuManager : IDomainService, IEventHandler<AddStoreEventData>
    {
        private readonly IRepository<Goods, long> _goodsRepository;
        private readonly IRepository<GoodsSkuAttribute, long> _goodsAttributeRepository;
        private readonly IRepository<SkuAttribute, int> _skuAttributeRepository;
        private readonly IRepository<SkuAttributeOption, int> _skuAttributeOptionRepository;
        private readonly IRepository<Products.Domain.Product, long> _productRepository;
        private readonly IRepository<ProductClassify, int> _classifyRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SkuManager(
            IRepository<Goods,long> goodsRepository, 
            IRepository<GoodsSkuAttribute, long> goodsAttributeRepository, 
            IRepository<Products.Domain.Product, long> productRepository,
            IRepository<SkuAttribute, int> skuAttributeRepository,
            IRepository<SkuAttributeOption, int> skuAttributeOptionRepository,
            IRepository<ProductClassify, int> classifyRepository)
        {
            _goodsRepository = goodsRepository;
            _skuAttributeRepository = skuAttributeRepository;
            _skuAttributeOptionRepository = skuAttributeOptionRepository;
            _goodsAttributeRepository = goodsAttributeRepository;
            _productRepository = productRepository;
            _classifyRepository = classifyRepository;
        }


        #region SKU属性

        /// <summary>
        /// 创建SKU属性
        /// </summary>
        /// <param name="skuAttribute">SKU属性</param>
        /// <returns></returns>
        public async Task CreateSkuAttributeAsync(SkuAttribute skuAttribute)
        {
            skuAttribute.CheckNotNull("skuAttribute");
            skuAttribute.Name.CheckNotNullOrEmpty("skuAttribute.Name");

            if (_skuAttributeRepository.CheckExists(p => p.Name == skuAttribute.Name))
            {
                throw new UserFriendlyException("该SKU属性已存在");
            }
            await _skuAttributeRepository.InsertAsync(skuAttribute);
        }

        /// <summary>
        /// 更新SKU属性
        /// </summary>
        /// <param name="skuAttribute">SKU属性</param>
        /// <returns></returns>
        public async Task UpdateSkuAttributeTempateAsync(SkuAttribute skuAttribute)
        {
            skuAttribute.CheckNotNull("skuAttribute");
            skuAttribute.Name.CheckNotNullOrEmpty("skuAttribute.Name");

            if (_skuAttributeRepository.CheckExists(p => p.Name == skuAttribute.Name, skuAttribute.Id))
            {
                throw new UserFriendlyException("该SKU属性已存在");
            }
            await _skuAttributeRepository.UpdateAsync(skuAttribute);
        }

        /// <summary>
        /// 获取 分类所有的SKU属性（包含父级和公共SKU属性）
        /// </summary>
        /// <param name="classify">分类</param>
        /// <returns></returns>
        public async Task<ICollection<SkuAttribute>> GetSkuAttributeByClassify(ProductClassify classify)
        {
            classify.CheckNotNull("classify");
            var classifyIds = classify.ParentIds.Split(",").Select(int.Parse).ToList();
            classifyIds.Add(classify.Id);

            return await _skuAttributeRepository.GetAllListAsync(p => p.ProductClassifyId == null || classifyIds.Contains(p.ProductClassifyId.Value));
        }

        /// <summary>
        /// 获取 分类所有的SKU属性（包含父级和公共SKU属性）
        /// </summary>
        /// <param name="classifyId">分类Id</param>
        /// <returns></returns>
        public async Task<ICollection<SkuAttribute>> GetSkuAttributeByClassify(int classifyId)
        {
            classifyId.CheckGreaterThan("classifyId", 0);
            var classify = await _classifyRepository.GetAsync(classifyId);
            return await GetSkuAttributeByClassify(classify);
        }

        #endregion

        #region SKU选项

        /// <summary>
        /// 创建SKU属性选项
        /// </summary>
        /// <param name="skuOption">SKU属性选项</param>
        /// <returns></returns>
        public async Task CreateSkuAttributeOptionAsync(SkuAttributeOption skuOption)
        {
            skuOption.CheckNotNull("skuOption");
            skuOption.Value.CheckNotNullOrEmpty("skuOption.Value");

            if (!_skuAttributeRepository.CheckExists(p => p.Id == skuOption.SkuAttributeId))
            {
                throw new UserFriendlyException("指定的SKU属性不存在");
            }

            if (_skuAttributeOptionRepository.CheckExists(p => p.SkuAttributeId == skuOption.SkuAttributeId && p.Value == skuOption.Value))
            {
                throw new UserFriendlyException("该SKU属性选项已存在");
            }
            await _skuAttributeOptionRepository.InsertAsync(skuOption);
        }

        /// <summary>
        /// 更新SKU属性选项
        /// </summary>
        /// <param name="skuOption">SKU属性选项</param>
        /// <returns></returns>
        public async Task UpdateSkuAttributeOptionAsync(SkuAttributeOption skuOption)
        {
            skuOption.CheckNotNull("skuOption");
            skuOption.Value.CheckNotNullOrEmpty("skuOption.Value");

            if (!_skuAttributeRepository.CheckExists(p => p.Id == skuOption.SkuAttributeId))
            {
                throw new UserFriendlyException("指定的SKU属性不存在");
            }

            if (_skuAttributeOptionRepository.CheckExists(p => p.SkuAttributeId == skuOption.SkuAttributeId && p.Value == skuOption.Value, skuOption.Id))
            {
                throw new UserFriendlyException("该SKU属性选项已存在");
            }
            await _skuAttributeOptionRepository.UpdateAsync(skuOption);
        }

        #endregion

        #region 货品

        /// <summary>
        /// 添加货品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="stock">库存</param>
        /// <param name="skuAttrValueIds">SKU属性值Id集合</param>
        /// <returns></returns>
        public async Task CreateGoodsAsync(long productId, int stock, int[] skuAttrValueIds)
        {
            productId.CheckGreaterThan("productId", 0);
            stock.CheckGreaterThan("stock", 0, true);
            if (!_productRepository.CheckExists(p => p.Id == productId))
            {
                throw new UserFriendlyException("所属的产品不存在");
            }

            if (!CheckSatisfySkuAttributes(skuAttrValueIds))
            {
                throw new UserFriendlyException("SKU属性验证失败，请检查输入");
            }

            if (await CheckDuplicateSkuAttributeOptions(productId, skuAttrValueIds))
            {
                throw new UserFriendlyException("指定SKU属性重复，请检查输入");
            }

            var goods = new Goods
            {
                ProductId = productId,
                Stock = stock,
                Details = skuAttrValueIds.Select(p => new GoodsSkuAttribute
                {
                    SkuAttributeOptionId = p
                }).ToArray()
            };
            await _goodsRepository.InsertAsync(goods);
        }

        private bool CheckSatisfySkuAttributes(int[] attrValueIds)
        {
            var skuAttrAttributeIds = _skuAttributeRepository
                .GetAll()
                .Select(p => p.Id)
                .OrderBy(p => p)
                .ToList()
                .ExpandAndToString();

            var checkedAttrAttributeIds = _skuAttributeOptionRepository
                .GetAll()
                .Where(p => attrValueIds.Contains(p.Id))
                .Select(p => p.SkuAttributeId)
                .OrderBy(p => p)
                .ToList()
                .ExpandAndToString();

            return skuAttrAttributeIds == checkedAttrAttributeIds;
        }

        private async Task<bool> CheckDuplicateSkuAttributeOptions(long productId, int[] attrValueIds)
        {
            var newIds = attrValueIds.OrderBy(p => p).ExpandAndToString();
            var existAttrValueIds = await _goodsRepository.QueryWithNoTracking()
                .Where(p => p.ProductId == productId)
                .Include(p => p.Details)
                .Select(p => p.Details.Select(m => m.SkuAttributeOptionId))
                .ToListAsync();

            return existAttrValueIds.Any(p => p.OrderBy(m => m).ExpandAndToString() == newIds);
        }

        #endregion
        
        #region 领域事件

        /// <summary>
        /// 新增库存事件
        /// </summary>
        /// <param name="eventData"></param>
        public void HandleEvent(AddStoreEventData eventData)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
