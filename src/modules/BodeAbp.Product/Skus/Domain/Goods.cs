using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Product.Skus.Domain
{
    /// <summary>
    /// 货品（最小存货单位）
    /// </summary>
    [Table("Product#Goods")]
    public class Goods : FullAuditedEntity<long>
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// SKU 属性组详情
        /// </summary>
        [ForeignKey("GoodsId")]
        public virtual ICollection<GoodsSkuAttribute> Details { get; set; }
    }
}
