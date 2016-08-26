using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Rpc.Ids;
using Abp.Rpc.Server.Attributes;
using Abp.Rpc.Services;

namespace Abp.Rpc.Server.ServiceDiscovery
{
    /// <summary>
    /// Clr服务条目工厂。
    /// </summary>
    public class ClrServiceEntryFactory : IClrServiceEntryFactory
    {
        #region Field

        protected internal IIocManager _iocManager;
        private readonly IServiceIdGenerator _serviceIdGenerator;

        #endregion Field

        #region Constructor

        public ClrServiceEntryFactory(IServiceIdGenerator serviceIdGenerator,IIocManager iocManager)
        {
            _iocManager = iocManager;
            _serviceIdGenerator = serviceIdGenerator;
        }

        #endregion Constructor

        #region Implementation of IClrServiceEntryFactory

        /// <summary>
        /// 创建服务条目。
        /// </summary>
        /// <param name="service">服务类型。</param>
        /// <param name="serviceImplementation">服务实现类型。</param>
        /// <returns>服务条目集合。</returns>
        public IEnumerable<ServiceEntry> CreateServiceEntry(Type service, Type serviceImplementation)
        {
            foreach (var methodInfo in service.GetTypeInfo().GetMethods())
            {
                var implementationMethodInfo = serviceImplementation.GetTypeInfo().GetMethod(methodInfo.Name, methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());
                yield return Create(methodInfo, implementationMethodInfo);
            }
        }

        #endregion Implementation of IClrServiceEntryFactory

        #region Private Method

        private ServiceEntry Create(MethodInfo method, MethodBase implementationMethod)
        {
            var serviceId = _serviceIdGenerator.GenerateServiceId(method);

            var serviceDescriptor = new ServiceDescriptor
            {
                Id = serviceId
            };

            var descriptorAttributes = method.GetCustomAttributes<RpcServiceDescriptorAttribute>();
            foreach (var descriptorAttribute in descriptorAttributes)
            {
                descriptorAttribute.Apply(serviceDescriptor);
            }

            return new ServiceEntry
            {
                Descriptor = serviceDescriptor,
                Func = parameters =>
                {
                    var instance = _iocManager.Resolve(method.DeclaringType);
                    var list = new List<object>();
                    foreach (var parameterInfo in implementationMethod.GetParameters())
                    {
                        var value = parameters[parameterInfo.Name];
                        var parameterType = parameterInfo.ParameterType;

                        var parameter = value.ToJsonString().FromJsonString(parameterType);
                        list.Add(parameter);
                    }

                    var result = implementationMethod.Invoke(instance, list.ToArray());
                    return Task.FromResult(result);
                }
            };
        }

        #endregion Private Method
    }
}
