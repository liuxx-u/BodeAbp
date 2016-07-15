using Abp.EntityFramework;
using BodeAbp.Product.Attributes.Domain;

namespace BodeAbp.Product.Attributes.ModelConfigs
{
    public class ProductClassifyConfiguration:EntityConfigurationBase<ProductClassify, int>
    {
        public ProductClassifyConfiguration()
        {
            HasOptional(p => p.Parent);
        }
    }
}
