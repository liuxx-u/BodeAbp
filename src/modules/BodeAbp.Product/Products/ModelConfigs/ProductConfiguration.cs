using Abp.EntityFramework;

namespace BodeAbp.Product.Products.ModelConfigs
{
    public class ProductConfiguration : EntityConfigurationBase<Domain.Product, long>
    {
        public ProductConfiguration()
        {
        }
    }
}
