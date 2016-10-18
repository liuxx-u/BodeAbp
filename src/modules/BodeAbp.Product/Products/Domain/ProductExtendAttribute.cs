using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 商品额外属性
    /// </summary>
    [Table("Product#ExtendAttribute")]
    public class ProductExtendAttribute : Entity<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }
    }
}
