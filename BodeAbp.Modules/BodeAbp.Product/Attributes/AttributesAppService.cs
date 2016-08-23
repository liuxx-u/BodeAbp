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

namespace BodeAbp.Product.Attributes
{
    /// <summary>
    ///  属性 服务
    /// </summary>
    public class AttributesAppService : ApplicationService,IAttributesAppService
    {
        /// <summary>
        /// 产品属性 领域服务
        /// </summary>
        public AttributeManager attributeManager { get; set; }

        private readonly IRepository<ProductClassify> _classifyRepository;
        private readonly IRepository<AttributeOption> _attributeValueRepository;
        private readonly IRepository<Domain.Attribute> _attributeTemplateRepository;

        public AttributesAppService(
            IRepository<Domain.Attribute> attributeTemplateRepository,
            IRepository<ProductClassify> classifyRepository,
            IRepository<AttributeOption> attributeValueRepository)
        {
            _classifyRepository = classifyRepository;
            _attributeValueRepository = attributeValueRepository;
            _attributeTemplateRepository = attributeTemplateRepository;
        }

        #region 属性模版

        /// <inheritdoc/>
        public async Task<PagedResultOutput<GetAttributeListOutput>> GetAttributePagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _attributeTemplateRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetAttributeListOutput>(total, list.MapTo<List<GetAttributeListOutput>>());
        }

        /// <inheritdoc/>
        public async Task<GetAttributeOutput> GetAttribute(int id)
        {
            var result = await _attributeTemplateRepository.GetAsync(id);
            return result.MapTo<GetAttributeOutput>();
        }

        /// <inheritdoc/>
        public async Task CreateAttribute(CreateAttributeInput input)
        {
            var attributeTemplate = input.MapTo<Domain.Attribute>();
            await attributeManager.CreateAttributeTempateAsync(attributeTemplate);
        }

        /// <inheritdoc/>
        public async Task UpdateAttribute(UpdateAttributeInput input)
        {
            var attributeTemplate = await _attributeTemplateRepository.GetAsync(input.Id);
            input.MapTo(attributeTemplate);
            await attributeManager.UpdateAttributeTempateAsync(attributeTemplate);
        }

        /// <inheritdoc/>
        public async Task DeleteAttribute(List<IdInput> input)
        {
            var ids = input.Select(p => p.Id);
            await _attributeTemplateRepository.DeleteAsync(p => ids.Contains(p.Id));
        }

        #endregion

        #region 属性值

        /// <inheritdoc/>
        public async Task<PagedResultOutput<GetAttributeOptionListOutput>> GetAttributeOptionPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _attributeValueRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetAttributeOptionListOutput>(total, list.MapTo<List<GetAttributeOptionListOutput>>());
        }

        /// <inheritdoc/>
        public async Task<GetAttributeOptionOutput> GetAttributeOption(int id)
        {
            var result = await _attributeValueRepository.GetAsync(id);
            return result.MapTo<GetAttributeOptionOutput>();
        }

        /// <inheritdoc/>
        public async Task CreateAttributeOption(CreateAttributeOptionInput input)
        {
            var attributeValue = input.MapTo<AttributeOption>();
            await attributeManager.CreateAttributeOptionAsync(attributeValue);
        }

        /// <inheritdoc/>
        public async Task UpdateAttributeOption(UpdateAttributeOptionInput input)
        {
            var attributeValue = await _attributeValueRepository.GetAsync(input.Id);
            input.MapTo(attributeValue);
            await attributeManager.UpdateAttributeOptionAsync(attributeValue);
        }

        /// <inheritdoc/>
        public async Task DeleteAttributeOption(List<IdInput> input)
        {
            var ids = input.Select(p => p.Id);
            await _attributeValueRepository.DeleteAsync(p => ids.Contains(p.Id));
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
        public async Task CreateClassify(ProductClassifyInput input)
        {
            var classify = input.MapTo<ProductClassify>();
            await attributeManager.CreateClassifyAsync(classify);
        }

        /// <inheritdoc/>
        public async Task UpdateClassify(ProductClassifyInput input)
        {
            var classify = await _classifyRepository.GetAsync(input.Id);
            input.MapTo(classify);
            await attributeManager.UpdateClassifyAsync(classify);
        }

        /// <inheritdoc/>
        public async Task DeleteClassify(int id)
        {
            await attributeManager.DeleteClassifyAsync(id);
        }

        #endregion
    }
}