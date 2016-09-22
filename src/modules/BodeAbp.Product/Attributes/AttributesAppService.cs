using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BodeAbp.Product.Attributes.Domain;
using BodeAbp.Product.Attributes.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System;
using Abp.Extensions;
using Abp;

namespace BodeAbp.Product.Attributes
{
    /// <summary>
    ///  属性 服务
    /// </summary>
    public class AttributesAppService : ApplicationService, IAttributesAppService
    {
        /// <summary>
        /// 产品属性 领域服务
        /// </summary>
        public AttributeManager attributeManager { protected get; set; }

        private readonly IRepository<ProductClassify> _classifyRepository;
        private readonly IRepository<Domain.Attribute> _attributeRepository;
        private readonly IRepository<AttributeOption> _attributeOptionRepository;

        public AttributesAppService(
            IRepository<Domain.Attribute> attributeTemplateRepository,
            IRepository<ProductClassify> classifyRepository,
            IRepository<AttributeOption> attributeValueRepository)
        {
            _classifyRepository = classifyRepository;
            _attributeOptionRepository = attributeValueRepository;
            _attributeRepository = attributeTemplateRepository;
        }

        #region 属性模版

        /// <inheritdoc/>
        public async Task<PagedResultOutput<AttributeDto>> GetAttributePagedList(QueryListPagedRequestInput input)
        {
            var query = _attributeRepository.GetAll();
            //第一次查询只显示公共属性
            if (input.FilterGroup == null || !input.FilterGroup.Rules.Any(p => p.Field == "productClassifyId"))
            {
                query = _attributeRepository.GetAll().Where(p => p.ProductClassifyId == 0);
            }

            int total;
            var list = await query.Where(input, out total).ToListAsync();
            return new PagedResultOutput<AttributeDto>(total, list.MapTo<List<AttributeDto>>());
        }

        /// <inheritdoc/>
        public async Task CreateAttribute(AttributeDto input)
        {
            var attributeTemplate = input.MapTo<Domain.Attribute>();
            await attributeManager.CreateAttributeTempateAsync(attributeTemplate);
        }

        /// <inheritdoc/>
        public async Task UpdateAttribute(AttributeDto input)
        {
            var attributeTemplate = await _attributeRepository.GetAsync(input.Id);
            input.MapTo(attributeTemplate);
            await attributeManager.UpdateAttributeTempateAsync(attributeTemplate);
        }

        /// <inheritdoc/>
        public async Task DeleteAttribute(IdInput input)
        {
            await _attributeRepository.DeleteAsync(input.Id);
        }

        /// <inheritdoc/>
        public async Task<ICollection<TreeOutPut>> GetOptionalAttributeTreeData()
        {
            var attrbutes = await _attributeRepository.GetAll().AsNoTracking()
                .Where(p => p.AttributeType == AttributeType.Switch)
                .OrderBy(p => p.OrderNo)
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

        /// <inheritdoc/>
        public async Task<ICollection<OperableAttributeGroupDto>> GetClassifyGroupAttributes(IdInput input)
        {
            var attributes = await attributeManager.GetAttributeByClassifyId(input.Id);
            //未设置分组名的属性默认为“基础属性”分组
            attributes.Where(p => p.GroupName.IsNullOrWhiteSpace()).ToList().ForEach(p => p.GroupName = "基础属性");

            var result = attributes.GroupBy(p => p.GroupName).Select(p =>
                new OperableAttributeGroupDto
                {
                    GroupName = p.Key,
                    Attributes = p.OrderBy(m => m.OrderNo).MapTo<List<OperableAttributeDto>>()
                }).ToList();
            return result;
        }

        #endregion

        #region 属性选项

        /// <inheritdoc/>
        public async Task<PagedResultOutput<AttributeOptionDto>> GetAttributeOptionPagedList(QueryListPagedRequestInput input)
        {
            int total;

            var query = _attributeOptionRepository.GetAll().AsNoTracking();
            //首次加载不显示属性选项
            if (input.FilterGroup == null || !input.FilterGroup.Rules.Any(p => p.Field == "attributeId"))
            {
                query = query.Where(p => false);
            }
            var list = await query.Where(input, out total).ToListAsync();
            return new PagedResultOutput<AttributeOptionDto>(total, list.MapTo<List<AttributeOptionDto>>());
        }

        /// <inheritdoc/>
        public async Task CreateAttributeOption(AttributeOptionDto input)
        {
            var attributeValue = input.MapTo<AttributeOption>();
            await attributeManager.CreateAttributeOptionAsync(attributeValue);
        }

        /// <inheritdoc/>
        public async Task UpdateAttributeOption(AttributeOptionDto input)
        {
            var attributeValue = await _attributeOptionRepository.GetAsync(input.Id);
            input.MapTo(attributeValue);
            await attributeManager.UpdateAttributeOptionAsync(attributeValue);
        }

        /// <inheritdoc/>
        public async Task DeleteAttributeOption(IdInput[] input)
        {
            var ids = input.Select(p => p.Id);
            await _attributeOptionRepository.DeleteAsync(p => ids.Contains(p.Id));
        }

        #endregion

        #region 分类

        /// <inheritdoc/>
        public async Task<ICollection<ProductClassifyListOutPut>> GetAllClassifies()
        {
            var classifies = await _classifyRepository.GetAll().OrderBy(p => p.OrderNo).Select(p => new ProductClassifyListOutPut
            {
                Id = p.Id,
                Name = p.Name,
                ParentId = p.ParentId
            }).ToListAsync();

            Func<int?, List<ProductClassifyListOutPut>> getChildren = null;
            getChildren = parentId =>
             {
                 if (classifies.Any(m => m.ParentId == parentId))
                 {
                     return classifies.Where(m => m.ParentId == parentId).Select(m => new ProductClassifyListOutPut()
                     {
                         Id = m.Id,
                         Name = m.Name,
                         ParentId = m.ParentId,
                         Children = getChildren(m.Id)
                     }).ToList();
                 }
                 return new List<ProductClassifyListOutPut>();
             };

            return getChildren(null);
        }

        /// <inheritdoc/>
        public async Task<TreeOutPut[]> GetClassifyTreeData()
        {
            var classifys = await _classifyRepository.GetAll().Select(p => new TreeOutPut()
            {
                Value = p.Id + "",
                Text = p.Name,
                ParentValue = p.ParentId == null ? "null" : p.ParentId + ""
            }).ToListAsync();

            classifys.Insert(0, new TreeOutPut() { Value = "0", Text = "公共属性", ParentValue = "null" });
            return classifys.ToArray();
        }

        /// <inheritdoc/>
        public async Task<ICollection<NameValue>> GetClassifySelectedOptions()
        {
            var classifies = await _classifyRepository.GetAll().AsNoTracking().Select(p => new { p.Id, p.Name, p.ParentIds }).ToListAsync();
            var result = classifies.Select(p =>
              {
                  var name = string.Empty;
                  if (!p.ParentIds.IsNullOrWhiteSpace())
                  {
                      foreach (var parentId in p.ParentIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
                      {
                          name += classifies.Single(m => m.Id == parentId).Name + "/";
                      }
                  }
                  name += p.Name;
                  return new NameValue(name, p.Id.ToString());
              });
            return result.OrderBy(p => p.Name).ToArray();
        }

        /// <inheritdoc/>
        public async Task CreateClassify(ProductClassifyInput input)
        {
            input.CheckNotNull("input");
            var classify = input.MapTo<ProductClassify>();
            await attributeManager.CreateClassifyAsync(classify);
        }

        /// <inheritdoc/>
        public async Task UpdateClassify(ProductClassifyInput input)
        {
            input.CheckNotNull("input");
            var classify = await _classifyRepository.GetAsync(input.Id);
            input.MapTo(classify);
            await attributeManager.UpdateClassifyAsync(classify);
        }

        /// <inheritdoc/>
        public async Task DeleteClassify(int id)
        {
            id.CheckGreaterThan("id", 0);
            await attributeManager.DeleteClassifyAsync(id);
        }

        /// <inheritdoc/>
        public async Task ClassifyUp(IdInput input)
        {
            input.CheckNotNull("input");
            await attributeManager.ClassifyUpAsync(input.Id);
        }

        /// <inheritdoc/>
        public async Task ClassifyDown(IdInput input)
        {
            input.CheckNotNull("input");
            await attributeManager.ClassifyDownAsync(input.Id);
        }

        #endregion
    }
}