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
using Abp.Events.Bus;
using BodeAbp.Product.Skus.Event;

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
        private readonly IRepository<Attribute> _attributeTemplateRepository;

        public AttributesAppService(IRepository<Attribute> attributeTemplateRepository, IRepository<ProductClassify> classifyRepository, IRepository<AttributeOption> attributeValueRepository)
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
            var attributeTemplate = input.MapTo<Attribute>();
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
        public async Task<PagedResultOutput<GetProductClassifyListOutput>> GetClassifyPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _classifyRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetProductClassifyListOutput>(total, list.MapTo<List<GetProductClassifyListOutput>>());
        }

        /// <inheritdoc/>
        public async Task<GetProductClassifyOutput> GetClassify(int id)
        {
            var result = await _classifyRepository.GetAsync(id);
            return result.MapTo<GetProductClassifyOutput>();
        }

        /// <inheritdoc/>
        public async Task CreateClassify(CreateProductClassifyInput input)
        {
            var classify = input.MapTo<ProductClassify>();
            await attributeManager.CreateClassifyAsync(classify);
        }

        /// <inheritdoc/>
        public async Task UpdateClassify(UpdateProductClassifyInput input)
        {
            var classify = await _classifyRepository.GetAsync(input.Id);
            input.MapTo(classify);
            await attributeManager.UpdateClassifyAsync(classify);
        }

        /// <inheritdoc/>
        public async Task DeleteClassify(int classifyId)
        {
            await attributeManager.DeleteClassifyAsync(classifyId);
        }

        #endregion
    }
}