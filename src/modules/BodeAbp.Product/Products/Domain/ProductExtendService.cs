using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 商品增值服务
    /// </summary>
    [Table("Product_ExtendService")]
    public class ProductExtendService : Entity<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// 是否按天计费
        /// </summary>
        public bool BillingByDay { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }
    }
}
