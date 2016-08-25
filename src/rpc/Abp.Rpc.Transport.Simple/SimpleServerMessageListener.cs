using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Abp.Rpc.Messages;
using Abp.Rpc.Transport.Codec;
using Abp.Rpc.Transport.Simple.Tcp.Server;

namespace Abp.Rpc.Transport.Simple
{
    public class SimpleServerMessageListener : IMessageListener, IDisposable
    {
        #region Field

        private readonly ITransportMessageDecoder _transportMessageDecoder;
        private readonly ITransportMessageEncoder _transportMessageEncoder;

        private TcpSocketSaeaServer _server;

        #endregion Field

        #region Constructor

        public SimpleServerMessageListener(ITransportMessageCodecFactory codecFactory)
        {
            _transportMessageEncoder = codecFactory.GetEncoder();
            _transportMessageDecoder = codecFactory.GetDecoder();
        }

        #endregion Constructor

        #region Implementation of IMessageListener

        public event ReceivedDelegate Received;

        /// <summary>
        /// 触发接收到消息事件。
        /// </summary>
        /// <param name="sender">消息发送者。</param>
        /// <param name="message">接收到的消息。</param>
        /// <returns>一个任务。</returns>
        public async Task OnReceived(IMessageSender sender, TransportMessage message)
        {
            if (Received == null)
                return;
            await Received(sender, message);
        }

        public async Task StartAsync(EndPoint endPoint)
        {
            _server = new TcpSocketSaeaServer((IPEndPoint)endPoint, async (session, data, offset, count) =>
            {
                //接收到数据包
                var message = _transportMessageDecoder.Decode(data.Skip(offset).Take(count).ToArray());
                //接收到消息
                var sender = new SimpleServerMessageSender(_transportMessageEncoder, session);
                await OnReceived(sender, message);
            });
            _server.Listen();

            await Task.FromResult(1);
            //await Task.CompletedTask;
        }

        #endregion Implementation of IMessageListener

        #region Implementation of IDisposable

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            _server.Dispose();
        }

        #endregion Implementation of IDisposable
    }
}
