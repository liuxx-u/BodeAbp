using Abp.Application.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using BodeAbp.Product.Skus.Dtos;
using Abp.Domain.Repositories;
using BodeAbp.Product.Skus.Domain;
using Abp.Application.Services.Query;
using System.Data.Entity;
using Abp.AutoMapper;
using BodeAbp.Product.Attributes.Domain;
using Abp.Extensions;
using System;

namespace BodeAbp.Product.Skus
{
    /// <summary>
    /// SKU应用服务
    /// </summary>
    public class SkusAppService : ApplicationService, ISkusAppService
    {
        /// <summary>
        /// SKU 领域服务
        /// </summary>
        public SkuManager skuManager { protected get; set; }

        private readonly IRepository<SkuAttribute> _skuAttributeRepository;
        private readonly IRepository<SkuAttributeOption> _skuAttributeOptionRepository;
        private readonly IRepository<ProductClassify> _classifyRepository;

        public SkusAppService(
            IRepository<SkuAttribute> skuAttributeRepository,
            IRepository<SkuAttributeOption> skuAttributeOptionRepository,
            IRepository<ProductClassify> classifyRepository)
        {
            _skuAttributeRepository = skuAttributeRepository;
            _skuAttributeOptionRepository = skuAttributeOptionRepository;
            _classifyRepository = classifyRepository;
        }

        #region SKU属性

        /// <inheritdoc/>
        public async Task<PagedResultOutput<SkuAttributeDto>> GetSkuAttributePagedList(QueryListPagedRequestInput input)
        {
            var query = _skuAttributeRepository.GetAll();
            //第一次查询只显示公共属性
            if (input.FilterGroup == null || !input.FilterGroup.Rules.Any(p => p.Field == "productClassifyId"))
            {
                query = _skuAttributeRepository.GetAll().Where(p => p.ProductClassifyId == 0);
            }

            int total;
            var list = await query.Where(input, out total).ToListAsync();
            return new PagedResultOutput<SkuAttributeDto>(total, list.MapTo<List<SkuAttributeDto>>());
        }

        /// <inheritdoc/>
        public async Task CreateSkuAttribute(SkuAttributeDto input)
        {
            var skuAttribute = input.MapTo<SkuAttribute>();
            await skuManager.CreateSkuAttributeAsync(skuAttribute);
        }

        /// <inheritdoc/>
        public async Task UpdateSkuAttribute(SkuAttributeDto input)
        {
            var skuAttribute = await _skuAttributeRepository.GetAsync(input.Id);
            input.MapTo(skuAttribute);
            await skuManager.UpdateSkuAttributeAsync(skuAttribute);
        }

        /// <inheritdoc/>
        public async Task DeleteSkuAttribute(IdInput input)
        {
            await _skuAttributeRepository.DeleteAsync(input.Id);
        }


        /// <inheritdoc/>
        public async Task<ICollection<TreeOutPut>> GetOptionalSkuAttributeTreeData()
        {
            var attrbutes = await _skuAttributeRepository.GetAll().AsNoTracking()
                .Select(p => new { p.Id, p.Name, p.ProductClassifyId }).ToListAsync();

            var classifyIds = attrbutes.Where(p => p.ProductClassifyId > 0).Select(p => p.ProductClassifyId).Distinct().ToList();
            var parentIds = await _classifyRepository.GetAll().AsNoTracking().Where(p => classifyIds.Contains(p.Id)).Select(p => p.ParentIds).ToListAsync();
            foreach (var pId in parentIds)
            {
                if (pId.IsNullOrWhiteSpace()) continue;
                var parentClassifyIds = pId.Split(",", StringSplitOptions.RemoveEmptyEntries);
                foreach (var classifyId in parentClassifyIds)
                {
                    int id;
                    if (int.TryParse(classifyId, out id) && !classifyIds.Contains(id)) classifyIds.Add(id);
                }
            }
            var treeClassifies = await _classifyRepository.GetAll().AsNoTracking()
                .Where(p => classifyIds.Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.ParentId,
                    p.ParentIds
                }).ToListAsync();

            var treeNodes = treeClassifies.Select(p =>
            {
                var level = p.ParentIds.IsNullOrWhiteSpace() ? 0 : p.ParentIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Length;
                return new TreeOutPut
                {
                    Value = string.Format("classify_{0}_{1}", level, p.Id),
                    Text = p.Name,
                    ParentValue = level == 0 ? "null" : string.Format("classify_{0}_{1}", level - 1, p.ParentId)
                };
            }).ToList();

            if (attrbutes.Any(p => p.ProductClassifyId == 0))
            {
                treeNodes.Insert(0, new TreeOutPut() { Value = "classify_0_0", Text = "公共属性", ParentValue = "null" });
            }
            treeNodes.AddRange(attrbutes.Select(p =>
            {
                var classify = treeClassifies.SingleOrDefault(m => m.Id == p.ProductClassifyId);
                var level = classify == null || classify.ParentIds.IsNullOrWhiteSpace() ? 0 : classify.ParentIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Length;
                return new TreeOutPut
                {
                    Value = p.Id + "",
                    Text = p.Name,
                    ParentValue = string.Format("classify_{0}_{1}", level, p.ProductClassifyId),
                };
            }));
            return treeNodes;
        }

        #endregion

        #region SKU属性选项


        /// <inheritdoc/>
        public async Task<PagedResultOutput<SkuAttributeOptionDto>> GetSkuAttributeOptionPagedList(QueryListPagedRequestInput input)
        {
            int total;

            var query = _skuAttributeOptionRepository.GetAll().AsNoTracking();
            //首次加载不显示SKU属性选项
            if (input.FilterGroup == null || !input.FilterGroup.Rules.Any(p => p.Field == "skuAttributeId"))
            {
                query = query.Where(p => false);
            }
            var list = await query.Where(input, out total).ToListAsync();
            return new PagedResultOutput<SkuAttributeOptionDto>(total, list.MapTo<List<SkuAttributeOptionDto>>());
        }

        /// <inheritdoc/>
        public async Task CreateSkuAttributeOption(SkuAttributeOptionDto input)
        {
            var skuAttributeOption = input.MapTo<SkuAttributeOption>();
            await skuManager.CreateSkuAttributeOptionAsync(skuAttributeOption);
        }

        /// <inheritdoc/>
        public async Task UpdateSkuAttributeOption(SkuAttributeOptionDto input)
        {
            var skuAttributeOption = await _skuAttributeOptionRepository.GetAsync(input.Id);
            input.MapTo(skuAttributeOption);
            await skuManager.UpdateSkuAttributeOptionAsync(skuAttributeOption);
        }

        /// <inheritdoc/>
        public async Task DeleteSkuAttributeOption(IdInput[] input)
        {
            var ids = input.Select(p => p.Id);
            await _skuAttributeOptionRepository.DeleteAsync(p => ids.Contains(p.Id));
        }

        #endregion
    }
}
