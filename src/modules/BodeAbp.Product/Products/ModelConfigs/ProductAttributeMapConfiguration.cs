using Abp.EntityFramework;
using BodeAbp.Product.Products.Domain;

namespace BodeAbp.Product.Products.ModelConfigs
{
    public class ProductAttributeMapConfiguration : EntityConfigurationBase<ProductAttributeMap, long>
    {
        public ProductAttributeMapConfiguration()
        {
            HasRequired(p => p.Attribute);
            HasRequired(p => p.Product).WithMany(p => p.Attributes).HasForeignKey(p => p.ProductId);
        }
    }
}
