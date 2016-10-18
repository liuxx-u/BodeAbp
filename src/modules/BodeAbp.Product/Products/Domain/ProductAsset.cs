using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Asset;

namespace BodeAbp.Product.Products.Domain
{
    /// <summary>
    /// 商品资源(图片、视频、文档等)
    /// </summary>
    [Table("Product#Asset")]
    public class ProductAsset : AssetEntity<long>
    {
        public long ProductId { get; set; }
    }
}
