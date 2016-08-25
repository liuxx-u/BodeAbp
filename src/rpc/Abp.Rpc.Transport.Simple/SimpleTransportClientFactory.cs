using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abp.Rpc.Server;
using Abp.Rpc.Transport;
using Abp.Rpc.Transport.Codec;
using Abp.Rpc.Transport.Simple.Tcp.Client;

namespace Abp.Rpc.Transport.Simple
{
    public class SimpleTransportClientFactory : ITransportClientFactory, IDisposable
    {
        private readonly ITransportMessageCodecFactory _transportMessageCodecFactory;
        private readonly IServiceExecutor _serviceExecutor;
        private readonly ConcurrentDictionary<string, Lazy<ITransportClient>> _clients = new ConcurrentDictionary<string, Lazy<ITransportClient>>();

        public SimpleTransportClientFactory(ITransportMessageCodecFactory transportMessageCodecFactory) : this(transportMessageCodecFactory, null)
        {
        }

        public SimpleTransportClientFactory(ITransportMessageCodecFactory transportMessageCodecFactory, IServiceExecutor serviceExecutor)
        {
            _transportMessageCodecFactory = transportMessageCodecFactory;
            _serviceExecutor = serviceExecutor;
        }

        #region Implementation of ITransportClientFactory

        /// <summary>
        /// 创建客户端。
        /// </summary>
        /// <param name="endPoint">终结点。</param>
        /// <returns>传输客户端实例。</returns>
        public ITransportClient CreateClient(EndPoint endPoint)
        {
            var key = endPoint.ToString();
            return _clients.GetOrAdd(key, k => new Lazy<ITransportClient>(() =>
            {
                var messageListener = new MessageListener();
                Func<TcpSocketSaeaClient> clientFactory = () =>
                {
                    var client = new TcpSocketSaeaClient((IPEndPoint)endPoint, async (c, data, offset, count) =>
                    {
                        //接收到数据包
                        var transportMessageDecoder = _transportMessageCodecFactory.GetDecoder();
                        var transportMessageEncoder = _transportMessageCodecFactory.GetEncoder();
                        var message = transportMessageDecoder.Decode(data.Skip(offset).Take(count).ToArray());

                        //接收到消息
                        await messageListener.OnReceived(new SimpleClientMessageSender(transportMessageEncoder, c), message);
                    });
                    client.Connect().Wait();
                    return client;
                };
                return new TransportClient(new SimpleClientMessageSender(_transportMessageCodecFactory.GetEncoder(), clientFactory), messageListener, _serviceExecutor);
            })).Value;
        }

        #endregion Implementation of ITransportClientFactory

        #region Implementation of IDisposable

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            foreach (var client in _clients.Values.Where(i => i.IsValueCreated))
            {
                (client.Value as IDisposable)?.Dispose();
            }
        }

        #endregion Implementation of IDisposable
    }
}
