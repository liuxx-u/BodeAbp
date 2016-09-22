using Abp.Net.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Net.Remoting.Args;
using BodeAbp.Queue.Clients.Consumers;
using BodeAbp.Queue.Protocols;
using System.Reflection;
using System.Collections.ObjectModel;
using Abp.Extensions;
using Abp.Application.Services;
using Abp.Serialization;
using Abp.Dependency;
using BodeAbp.Queue.Application.Protocols;

namespace BodeAbp.Queue.Application.RequestHandlers
{
    public class ApplicationServiceInvokeRequestHandler : IMessageHandler
    {
        private readonly ReadOnlyCollection<Type> _applicationTypes;
        private ISerializer<byte[]> _binarySerializer;

        public ApplicationServiceInvokeRequestHandler() : this(AppDomain.CurrentDomain.GetAssemblies()) { }

        public ApplicationServiceInvokeRequestHandler(Assembly[] assemblies)
        {
            var applicationTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.DefinedTypes.Where(p => typeof(IApplicationService).IsAssignableFrom(p));
                applicationTypes.AddRange(types);
            }
            _applicationTypes = new ReadOnlyCollection<Type>(applicationTypes);
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
        }


        public void Handle(QueueMessage message, IMessageContext context)
        {
            var invokeMessage = _binarySerializer.Deserialize<byte[], ApplicationInvokeMessage>(message.Body);
            var applicationType = _applicationTypes.Where(p => p.FullName == invokeMessage.TypeName).SingleOrDefault();
            if (applicationType == null)
            {
                //未找到类型为“{0}”的服务
            }

            var method = applicationType.GetMethods().SingleOrDefault(p => p.Name == invokeMessage.MethodName);
            if (method == null)
            {
                //未找到类型为“{0}”的“{1}”服务
            }

            var instance = IocManager.Instance.Resolve(applicationType);
            var result = method.Invoke(instance, invokeMessage.Args);

            context.OnMessageHandled(message);
        }
    }
}
