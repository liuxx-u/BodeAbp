using Abp.Configuration.Startup;

namespace Abp.Rpc.Configuration
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP Web module.
    /// </summary>
    public static class AbpRpcConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP Rpc module.
        /// </summary>
        public static IAbpRpcModuleConfiguration AbpRpc(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.GetOrCreate("Modules.Abp.Rpc", () => configurations.AbpConfiguration.IocManager.Resolve<IAbpRpcModuleConfiguration>());
        }
    }
}
