using Abp.EntityFramework;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Attributes.ModelConfigs
{
    public class AttributeOptionConfiguration:EntityConfigurationBase<AttributeOption,int>
    {
        public AttributeOptionConfiguration()
        {
            HasRequired(p => p.Attribute).WithMany(p => p.Options).HasForeignKey(p => p.AttributeId);
        }
    }
}
