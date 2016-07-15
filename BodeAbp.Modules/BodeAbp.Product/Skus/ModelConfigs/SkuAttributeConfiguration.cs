using Abp.EntityFramework;
using BodeAbp.Product.Skus.Domain;

namespace BodeAbp.Product.Attributes.ModelConfigs
{
    public class SkuAttributeConfiguration : EntityConfigurationBase<SkuAttribute, int>
    {
        public SkuAttributeConfiguration()
        {
            HasOptional(p => p.ProductClassify);
        }
    }
}
