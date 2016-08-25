using Abp.EntityFramework;
using BodeAbp.Product.Skus.Domain;

namespace BodeAbp.Product.Attributes.ModelConfigs
{
    public class SkuAttributeOptionConfiguration : EntityConfigurationBase<SkuAttributeOption, int>
    {
        public SkuAttributeOptionConfiguration()
        {
            HasRequired(p => p.SkuAttribute).WithMany(p => p.Options).HasForeignKey(p => p.SkuAttributeId);
        }
    }
}
