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
        public async Task<PagedResultOutput<GetAttributeListOutput>> GetAttributePagedList(QueryListPagedRequestInput input)
        {
            var query = _attributeRepository.GetAll();
            //第一次查询只显示公共属性
            if (input.FilterGroup == null || !input.FilterGroup.Rules.Any(p => p.Field == "productClassifyId"))
            {
                query = _attributeRepository.GetAll().Where(p => p.ProductClassifyId == null);
            }

            int total;
            var list = await query.Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetAttributeListOutput>(total, list.MapTo<List<GetAttributeListOutput>>());
        }

        /// <inheritdoc/>
        public async Task<GetAttributeOutput> GetAttribute(int id)
        {
            var result = await _attributeRepository.GetAsync(id);
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
            var attributeTemplate = await _attributeRepository.GetAsync(input.Id);
            input.MapTo(attributeTemplate);
            await attributeManager.UpdateAttributeTempateAsync(attributeTemplate);
        }

        /// <inheritdoc/>
        public async Task DeleteAttribute(IdInput input)
        { 
            await _attributeRepository.DeleteAsync(input.Id);
        }

        //public async Task<ICollection<TreeOutPut>> GetOptionalAttributeTreeData()
        //{
        //    var attrbutes = await _attributeRepository.GetAll().AsNoTracking()
        //        .Where(p => p.AttributeType == AttributeType.Switch || p.AttributeType == AttributeType.DropDown)
        //        .Select(p => new { p.Id, p.Name, p.ProductClassifyId }).ToListAsync();


        //}

        #endregion

        #region 属性值

        /// <inheritdoc/>
        public async Task<PagedResultOutput<GetAttributeOptionListOutput>> GetAttributeOptionPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await _attributeOptionRepository.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetAttributeOptionListOutput>(total, list.MapTo<List<GetAttributeOptionListOutput>>());
        }

        /// <inheritdoc/>
        public async Task<GetAttributeOptionOutput> GetAttributeOption(int id)
        {
            var result = await _attributeOptionRepository.GetAsync(id);
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