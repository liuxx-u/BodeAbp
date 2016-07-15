using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using WebDemo.Authorization.Roles;
using WebDemo.Application.Editions;
using WebDemo.Authorization.Users;
using Abp.Application.Features;

namespace WebDemo.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore
            )
            : base(
                tenantRepository,
                tenantFeatureRepository,
                editionManager,
                featureValueStore
            )
        {
        }
    }
}