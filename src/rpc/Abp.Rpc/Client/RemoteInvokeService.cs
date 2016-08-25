using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.Rpc.Client.Address;
using Abp.Rpc.Client.HealthChecks;
using Abp.Rpc.Exceptions;
using Abp.Rpc.Messages;
using Abp.Rpc.Transport;

namespace Abp.Rpc.Client
{
    public class RemoteInvokeService : IRemoteInvokeService
    {
        private readonly IAddressResolver _addressResolver;
        private readonly ITransportClientFactory _transportClientFactory;
        private readonly IHealthCheckService _healthCheckService;

        public RemoteInvokeService(IAddressResolver addressResolver, ITransportClientFactory transportClientFactory, IHealthCheckService healthCheckService)
        {
            _addressResolver = addressResolver;
            _transportClientFactory = transportClientFactory;
            _healthCheckService = healthCheckService;
        }

        #region Implementation of IRemoteInvokeService

        public Task<RemoteInvokeResultMessage> InvokeAsync(RemoteInvokeContext context)
        {
            return InvokeAsync(context, Task.Factory.CancellationToken);
        }

        public async Task<RemoteInvokeResultMessage> InvokeAsync(RemoteInvokeContext context, CancellationToken cancellationToken)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.InvokeMessage == null)
                throw new ArgumentNullException(nameof(context.InvokeMessage));

            if (string.IsNullOrEmpty(context.InvokeMessage.ServiceId))
                throw new ArgumentException("服务Id不能为空。", nameof(context.InvokeMessage.ServiceId));

            var invokeMessage = context.InvokeMessage;
            var address = await _addressResolver.Resolver(invokeMessage.ServiceId);

            if (address == null)
                throw new Exception($"无法解析服务Id：{invokeMessage.ServiceId}的地址信息。");

            try
            {
                var endPoint = address.CreateEndPoint();

                var client = _transportClientFactory.CreateClient(endPoint);
                return await client.SendAsync(context.InvokeMessage);
            }
            catch (RpcCommunicationException)
            {
                await _healthCheckService.MarkFailure(address);
                throw;
            }
            catch (Exception exception)
            {
                //$"发起请求中发生了错误，服务Id：{invokeMessage.ServiceId}。"
                throw;
            }
        }

        #endregion Implementation of IRemoteInvokeService
    }
}
