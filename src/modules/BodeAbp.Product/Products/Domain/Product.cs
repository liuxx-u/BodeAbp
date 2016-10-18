using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using BodeAbp.Product.Attributes.Domain;
using BodeAbp.Product.Skus.Domain;
using Abp;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 产品
    /// </summary>
    [Table("Product#Product")]
    public class Product : FullAuditedEntity<long>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Product()
        {
            Assets = new List<ProductAsset>();
            Attributes = new List<ProductAttribute>();
            ExtendAttributes = new List<ProductExtendAttribute>();
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        [StringLength(AbpStringLength.MaxLength64)]
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsOnShelf { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime? OnShelfTime { get; set; }

        /// <summary>
        /// 下架时间
        /// </summary>
        public DateTime? OffShelfTime { get; set; }

        /// <summary>
        /// 热度
        /// </summary>
        public int HotNo { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesNo { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public int ClassifyId { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [ForeignKey("ClassifyId")]
        public virtual ProductClassify Classify { get; set; }

        /// <summary>
        /// 资源集合
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductAsset> Assets { get; set; }

        /// <summary>
        /// 属性集合
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductAttribute> Attributes { get; set; }

        /// <summary>
        /// 额外属性集合
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductExtendAttribute> ExtendAttributes { get; set; }
    }
}
