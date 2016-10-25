using System;

namespace BodeAbp.Scaffolding
{
    internal static class IServiceProviderExtensions
    {
        public static TService GetService<TService>(this IServiceProvider provider) where TService : class
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            return (TService)provider.GetService(typeof(TService));
        }

        public static bool IsServiceAvailable<TService>(this IServiceProvider provider) where TService : class
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            return GetService<TService>(provider) != null;
        }
    }
}
