using Abp.Configuration.Startup;

namespace BodeAbp.Queue.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP Web module.
    /// </summary>
    public static class AbpQueueConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Rpc module.
        /// </summary>
        public static IAbpQueueModuleConfiguration AbpQueue(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.GetOrCreate("Modules.Abp.Queue", () => configurations.AbpConfiguration.IocManager.Resolve<IAbpQueueModuleConfiguration>());
        }
    }
}
