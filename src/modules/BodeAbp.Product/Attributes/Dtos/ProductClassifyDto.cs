using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Abp.AutoMapper;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Attributes.Dtos
{
    public abstract class ProductClassifyDto : EntityDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int? ParentId { get; set; }
    }

    public class ProductClassifyListOutPut: ProductClassifyDto,IOutputDto
    {
        public ProductClassifyListOutPut()
        {
            Children = new List<ProductClassifyListOutPut>();
        }
        
        /// <summary>
        /// 子分类
        /// </summary>
        public ICollection<ProductClassifyListOutPut> Children { get; set; }
    }

    [AutoMapTo(typeof(ProductClassify))]
    public class ProductClassifyInput : ProductClassifyDto, IInputDto
    {

    }
}


