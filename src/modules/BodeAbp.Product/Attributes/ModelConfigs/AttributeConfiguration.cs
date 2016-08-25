using Abp.EntityFramework;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Attributes.ModelConfigs
{
    public class AttributeConfiguration : EntityConfigurationBase<Attribute,int>
    {
        public AttributeConfiguration()
        {
            HasOptional(p => p.ProductClassify);
        }
    }
}
