using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using BodeAbp.Product.Attributes.Domain;
using Abp;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 产品
    /// </summary>
    [Table("Product_Product")]
    public class Product : FullAuditedEntity<long>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Product()
        {
            Assets = new List<ProductAsset>();
            Attributes = new List<ProductAttributeMap>();
            ExtendServices = new List<ProductExtendService>();
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        [StringLength(AbpStringLength.MaxLength64)]
        public string Name { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginPrice { get; set; }

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
        /// 库存
        /// </summary>
        public int Inventory { get; set; }

        /// <summary>
        /// 所在地区 Id
        /// </summary>
        public int RegionId { get; set; }

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
        public virtual ICollection<ProductAttributeMap> Attributes { get; set; }

        /// <summary>
        /// 增值服务集合
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductExtendService> ExtendServices { get; set; }
    }
}
