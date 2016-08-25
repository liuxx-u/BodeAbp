using System;
using Abp.Dependency;
using Abp.Rpc.Client;
using Abp.Rpc.Client.Address;
using Abp.Rpc.Client.HealthChecks;
using Abp.Rpc.Ids;
using Abp.Rpc.Routing;
using Abp.Rpc.Server;
using Abp.Rpc.Server.Attributes;
using Abp.Rpc.Server.ServiceDiscovery;
using Abp.Rpc.Transport;
using Abp.Rpc.Transport.Codec;

namespace Abp.Rpc.Configuration
{
    public class AbpRpcModuleConfiguration : IAbpRpcModuleConfiguration
    {
        public IIocManager IocManager { get; set; }

        public IAbpRpcModuleConfiguration InitRpcCore()
        {
            IocManager.Register<IServiceIdGenerator, DefaultServiceIdGenerator>();
            IocManager.Register<IServiceRouteFactory, DefaultServiceRouteFactory>();
            return this;
        }

        public IAbpRpcModuleConfiguration UseJsonCodec()
        {
            IocManager.Register<ITransportMessageCodecFactory, JsonTransportMessageCodecFactory>();
            return this;
        }

        public IAbpRpcModuleConfiguration InitClientRuntime()
        {
            IocManager.Register<IHealthCheckService, DefaultHealthCheckService>();
            IocManager.Register<IAddressResolver, DefaultAddressResolver>();
            IocManager.Register<IRemoteInvokeService, RemoteInvokeService>();
            return this;
        }

        public IAbpRpcModuleConfiguration InitServerRuntime()
        {
            IocManager.Register<IClrServiceEntryFactory, ClrServiceEntryFactory>();
            IocManager.Register<IServiceEntryProvider, AttributeServiceEntryProvider>();
            IocManager.Register<IServiceEntryManager, DefaultServiceEntryManager>();
            IocManager.Register<IServiceEntryLocate, DefaultServiceEntryLocate>();
            IocManager.Register<IServiceExecutor, DefaultServiceExecutor>();
            return this;
        }

        public IAbpRpcModuleConfiguration UsePollingAddressSelector()
        {
            IocManager.Register<IAddressSelector, PollingAddressSelector>();
            return this;
        }

        public IAbpRpcModuleConfiguration UseRandomAddressSelector()
        {
            IocManager.Register<IAddressSelector, RandomAddressSelector>();
            return this;
        }

        public IAbpRpcModuleConfiguration UseSharedFileRouteManager()
        {
            IocManager.Register<IServiceRouteManager, SharedFileServiceRouteManager>();
            return this;
        }
    }
}
