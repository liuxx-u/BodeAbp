using Abp.Rpc.Configuration;
using Abp.Rpc.Routing;

namespace Abp.Rpc.Coordinate.Zookeeper
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 使用ZoomKeeper进行路由管理。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IAbpRpcModuleConfiguration UseZooKeeperRouteManager(this IAbpRpcModuleConfiguration config)
        {
            config.IocManager.Register<IServiceRouteManager, ZooKeeperServiceRouteManager>();

            return config;
        }
    }
}
