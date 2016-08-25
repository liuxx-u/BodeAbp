using Abp.EntityFramework;
using BodeAbp.Product.Products.Domain;

namespace BodeAbp.Product.Products.ModelConfigs
{
    public class ProductAttributeConfiguration : EntityConfigurationBase<ProductAttribute, long>
    {
        public ProductAttributeConfiguration()
        {
            HasRequired(p => p.Product).WithMany(p => p.Attributes).HasForeignKey(p => p.ProductId);
        }
    }
}
