using Abp.EntityFramework;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Attributes.ModelConfigs
{
    public class ProductAttributeOptionConfiguration:EntityConfigurationBase<ProductAttributeOption,int>
    {
        public ProductAttributeOptionConfiguration()
        {
            HasRequired(p => p.Attribute).WithMany(p => p.AttributeOptions).HasForeignKey(p => p.AttributeId);
        }
    }
}
