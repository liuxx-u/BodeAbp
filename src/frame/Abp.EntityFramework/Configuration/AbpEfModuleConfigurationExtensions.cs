namespace Abp.Configuration.Startup
{
    public static class AbpEfModuleConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP EntityFramework module.
        /// </summary>
        public static IAbpEfModuleConfiguration AbpEntityFramework(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpEfModuleConfiguration>();
        }
    }
}
