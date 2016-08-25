using Abp.Application.Features;
using WebDemo.Authorization.Roles;
using WebDemo.MultiTenancy;
using WebDemo.Authorization.Users;
using Abp.Runtime.Caching;
using Abp.MultiTenancy;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace WebDemo.Application.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(
            ICacheManager cacheManager,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<EditionFeatureSetting, long> editionFeatureRepository,
            IFeatureManager featureManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(cacheManager,
                  tenantFeatureRepository,
                  tenantRepository,
                  editionFeatureRepository,
                  featureManager,
                  unitOfWorkManager)
        {
        }
    }
}