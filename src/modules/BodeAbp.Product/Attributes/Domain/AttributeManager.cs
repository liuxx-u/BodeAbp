using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.UI;
using System.Linq;

namespace BodeAbp.Product.Attributes.Domain
{
    /// <summary>
    /// 产品属性 领域服务
    /// </summary>
    public class AttributeManager : IDomainService
    {
        private readonly IRepository<ProductClassify> _classifyRepository;
        private readonly IRepository<AttributeOption> _attributeOptionRepository;
        private readonly IRepository<Attribute> _attributeRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AttributeManager(
            IRepository<Attribute> attributeRepository,
            IRepository<ProductClassify> classifyRepository,
            IRepository<AttributeOption> attributeOptionRepository)
        {
            _classifyRepository = classifyRepository;
            _attributeOptionRepository = attributeOptionRepository;
            _attributeRepository = attributeRepository;
        }

        #region 属性

        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="attribute">模版</param>
        /// <returns></returns>
        public async Task CreateAttributeTempateAsync(Attribute attribute)
        {
            attribute.CheckNotNull("attribute");
            attribute.Name.CheckNotNullOrEmpty("attribute.Name");

            if (_attributeRepository.CheckExists(p=>p.Name== attribute.Name))
            {
                throw new UserFriendlyException("该属性已存在");
            }
            await _attributeRepository.InsertAsync(attribute);
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="attribute">模版</param>
        /// <returns></returns>
        public async Task UpdateAttributeTempateAsync(Attribute attribute)
        {
            attribute.CheckNotNull("attribute");
            attribute.Name.CheckNotNullOrEmpty("attribute.Name");

            if (_attributeRepository.CheckExists(p => p.Name == attribute.Name, attribute.Id))
            {
                throw new UserFriendlyException("该属性已存在");
            }
            await _attributeRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 获取 分类所有的属性（包含父级和公共属性）
        /// </summary>
        /// <param name="classify">分类</param>
        /// <returns></returns>
        public async Task<ICollection<Attribute>> GetAttributeByClassify(ProductClassify classify)
        {
            classify.CheckNotNull("Classify");
            var classifyIds = classify.ParentIds.Split(",").Select(int.Parse).ToList();
            classifyIds.Add(classify.Id);

            return await _attributeRepository.GetAllListAsync(p => p.ProductClassifyId == null || classifyIds.Contains(p.ProductClassifyId.Value));
        }

        /// <summary>
        /// 获取 分类所有的属性（包含父级和公共属性）
        /// </summary>
        /// <param name="classifyId">分类Id</param>
        /// <returns></returns>
        public async Task<ICollection<Attribute>> GetAttributeByClassifyId(int classifyId)
        {
            classifyId.CheckGreaterThan("classifyId", 0);
            var classify = await _classifyRepository.GetAsync(classifyId);
            return await GetAttributeByClassify(classify);
        }

        #endregion

        #region 属性值

        /// <summary>
        /// 创建属性选项
        /// </summary>
        /// <param name="option">属性选项</param>
        /// <returns></returns>
        public async Task CreateAttributeOptionAsync(AttributeOption option)
        {
            option.CheckNotNull("option");
            option.Value.CheckNotNullOrEmpty("option.Value");

            if (!_attributeRepository.CheckExists(p => p.Id == option.AttributeId))
            {
                throw new UserFriendlyException("指定的属性不存在");
            }

            if (_attributeOptionRepository.CheckExists(p =>p.AttributeId== option.AttributeId&& p.Value == option.Value))
            {
                throw new UserFriendlyException("该属性选项已存在");
            }
            await _attributeOptionRepository.InsertAsync(option);
        }

        /// <summary>
        /// 更新属性选项
        /// </summary>
        /// <param name="option">属性值</param>
        /// <returns></returns>
        public async Task UpdateAttributeOptionAsync(AttributeOption option)
        {
            option.CheckNotNull("option");
            option.Value.CheckNotNullOrEmpty("option.Value");

            if (!_attributeRepository.CheckExists(p => p.Id == option.AttributeId))
            {
                throw new UserFriendlyException("指定的属性不存在");
            }

            if (_attributeOptionRepository.CheckExists(p => p.AttributeId == option.AttributeId && p.Value == option.Value, option.Id))
            {
                throw new UserFriendlyException("该属性选项已存在");
            }
            await _attributeOptionRepository.UpdateAsync(option);
        }

        /// <summary>
        /// 获取属性下所以的属性选项
        /// </summary>
        /// <param name="attributeId">属性Id</param>
        /// <returns></returns>
        public async Task<ICollection<AttributeOption>> GetAttributeOptionsByAttributeId(int attributeId)
        {
            return await _attributeOptionRepository.GetAllListAsync(p => p.AttributeId == attributeId);
        }

        #endregion

        #region 分类

        /// <summary>
        /// 创建  分类
        /// </summary>
        /// <param name="classify">分类</param>
        /// <returns></returns>
        public async Task CreateClassifyAsync(ProductClassify classify)
        {
            classify.CheckNotNull("classify");

            await CheckAndSetParentIds(classify);
            await _classifyRepository.InsertAsync(classify);
        }

        /// <summary>
        /// 更新  分类
        /// </summary>
        /// <param name="classify">分类</param>
        /// <returns></returns>
        public async Task UpdateClassifyAsync(ProductClassify classify)
        {
            classify.CheckNotNull("classify");

            await CheckAndSetParentIds(classify);
            await _classifyRepository.UpdateAsync(classify);
        }

        /// <summary>
        /// 删除  分类，同时删除所有子分类
        /// </summary>
        /// <param name="classifyId">分类Id</param>
        /// <returns></returns>
        public async Task DeleteClassifyAsync(int classifyId)
        {
            classifyId.CheckGreaterThan("classifyId", 0);
            var classify = await _classifyRepository.GetAsync(classifyId);
            await DeleteClassifyAsync(classify);
        }

        /// <summary>
        /// 删除  分类，同时删除所有子分类
        /// </summary>
        /// <param name="classify">分类</param>
        /// <returns></returns>
        public async Task DeleteClassifyAsync(ProductClassify classify)
        {
            classify.CheckNotNull("classify");
            await _classifyRepository.DeleteAsync(classify);

            //删除所有子分类
            string parentIds = classify.ParentIds + classify.Id + ",";
            await _classifyRepository.DeleteAsync(p => p.ParentIds.StartsWith(parentIds));
        }

        private async Task CheckAndSetParentIds(ProductClassify classify)
        {
            if (_classifyRepository.CheckExists(p => p.ParentId == classify.ParentId && p.Name == classify.Name, classify.Id))
            {
                throw new UserFriendlyException("该分类已存在");
            }

            if (classify.ParentId.HasValue)
            {
                if (classify.ParentId.Value == 0)
                {
                    classify.ParentId = null;
                }
                else
                {
                    var parent = await _classifyRepository.GetAsync(classify.ParentId.Value);
                    if (parent == null)
                    {
                        throw new UserFriendlyException("指定的父级分类不存在");
                    }

                    classify.ParentIds = parent.ParentIds + parent.Id.ToString() + ",";
                }
            }
        }

        #endregion

    }
}
