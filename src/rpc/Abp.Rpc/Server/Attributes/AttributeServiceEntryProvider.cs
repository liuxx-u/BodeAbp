using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Rpc.Server.ServiceDiscovery;

namespace Abp.Rpc.Server.Attributes
{
    /// <summary>
    /// Service标记类型的服务条目提供程序。
    /// </summary>
    public class AttributeServiceEntryProvider : IServiceEntryProvider
    {
        #region Field

        private readonly IEnumerable<Type> _types;
        private readonly IClrServiceEntryFactory _clrServiceEntryFactory;

        #endregion Field

        #region Constructor

        public AttributeServiceEntryProvider(IClrServiceEntryFactory clrServiceEntryFactory)
        {
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            _types = assemblys.Where(i => i.IsDynamic == false).SelectMany(i => i.ExportedTypes).ToArray();
            _clrServiceEntryFactory = clrServiceEntryFactory;
        }

        #endregion Constructor

        #region Implementation of IServiceEntryProvider

        /// <summary>
        /// 获取服务条目集合。
        /// </summary>
        /// <returns>服务条目集合。</returns>
        public IEnumerable<ServiceEntry> GetEntries()
        {
            var services = _types.Where(i =>
            {
                var typeInfo = i.GetTypeInfo();
                return typeInfo.IsInterface && typeInfo.GetCustomAttribute<RpcServiceBundleAttribute>() != null;
            }).ToArray();
            var serviceImplementations = _types.Where(i =>
            {
                var typeInfo = i.GetTypeInfo();
                return typeInfo.IsClass && !typeInfo.IsAbstract && i.Namespace != null && !i.Namespace.StartsWith("System") &&
                !i.Namespace.StartsWith("Microsoft");
            }).ToArray();

            //if (_logger.IsEnabled(LogLevel.Information))
            //{
            //    _logger.LogInformation($"发现了以下服务：{string.Join(",", services.Select(i => i.ToString()))}。");
            //}

            var entries = new List<ServiceEntry>();
            foreach (var service in services)
            {
                foreach (var serviceImplementation in serviceImplementations.Where(i => service.GetTypeInfo().IsAssignableFrom(i)))
                {
                    entries.AddRange(_clrServiceEntryFactory.CreateServiceEntry(service, serviceImplementation));
                }
            }
            return entries;
        }

        #endregion Implementation of IServiceEntryProvider
    }
}
