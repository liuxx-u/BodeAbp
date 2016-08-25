using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Rpc.Configuration;
using Abp.Rpc.Server;

namespace Abp.Rpc.Transport.Simple
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 使用Simple进行传输。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IAbpRpcModuleConfiguration UseSimpleTransport(this IAbpRpcModuleConfiguration config)
        {
            config.IocManager.Register<ITransportClientFactory, SimpleTransportClientFactory>();
            config.IocManager.Register<IMessageListener, SimpleServerMessageListener>();
            config.IocManager.Register<IServiceHost, DefaultServiceHost>();

            return config;
        }
    }
}
