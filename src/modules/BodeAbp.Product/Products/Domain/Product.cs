using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using BodeAbp.Product.Attributes.Domain;
using BodeAbp.Product.Skus.Domain;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 产品
    /// </summary>
    [Table("Product#Product")]
    public class Product : FullAuditedEntity<long>
    {
        #region 常量

        /// <summary>
        /// 商品名称<see cref="Name"/>最大长度
        /// </summary>
        public const int MaxNameLength = 64;

        /// <summary>
        /// 商品简介<see cref="Brief"/>最大长度
        /// </summary>
        public const int MaxBriefLength = 128;

        /// <summary>
        /// 商品编号<see cref="ProductNo"/>最大长度
        /// </summary>
        public const int MaxProductNoLength = 32;

        #endregion

        #region 属性

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 商品简介
        /// </summary>
        [StringLength(MaxBriefLength)]
        public string Brief { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        [StringLength(MaxProductNoLength)]
        public string ProductNo { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 优惠价
        /// </summary>
        public decimal PreferentialPrice { get; set; }

        /// <summary>
        /// 封面(1:1)
        /// </summary>
        public string Cover1 { get; set; }

        /// <summary>
        /// 封面(16:9)
        /// </summary>
        public string Cover2 { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsOnShelf { get; set; }

        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime? OnShelfTime { get; set; }

        /// <summary>
        /// 热度
        /// </summary>
        public int HotNo { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesNo { get; set; }

        /// <summary>
        /// 枚举测试
        /// </summary>
        public EnumTest EnumTest { get; set; }

        ///// <summary>
        ///// 分类Id
        ///// </summary>
        //public int ClassifyId { get; set; }

        ///// <summary>
        ///// 分类
        ///// </summary>
        //[ForeignKey("ClassifyId")]
        //public virtual ProductClassify Classify { get; set; }

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

        /// <summary>
        /// SKU集合
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<Goods> Skus { get; set; }

        #endregion
    }

    /// <summary>
    /// 枚举测试
    /// </summary>
    public enum EnumTest
    {
        男装 = 1,
        女装 = 2,
        童装 = 3
    }
}
