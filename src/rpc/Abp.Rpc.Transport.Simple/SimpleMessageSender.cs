using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Rpc.Messages;
using Abp.Rpc.Transport.Codec;
using Abp.Rpc.Transport.Simple.Tcp;
using Abp.Rpc.Transport.Simple.Tcp.Client;
using Abp.Rpc.Transport.Simple.Tcp.Server;

namespace Abp.Rpc.Transport.Simple
{
    public abstract class SimpleMessageSender
    {
        private readonly ITransportMessageEncoder _transportMessageEncoder;

        protected SimpleMessageSender(ITransportMessageEncoder transportMessageEncoder)
        {
            _transportMessageEncoder = transportMessageEncoder;
        }

        protected byte[] GetByteBuffer(TransportMessage message)
        {
            var data = _transportMessageEncoder.Encode(message);

            return data;
        }
    }

    public class SimpleClientMessageSender : SimpleMessageSender, IMessageSender
    {
        private readonly Func<TcpSocketSaeaClient> _clientFactory;
        private TcpSocketSaeaClient _client;

        private TcpSocketSaeaClient Client
        {
            get
            {
                if (_client != null && _client.State != TcpSocketConnectionState.Closed || _clientFactory == null)
                    return _client;
                lock (this)
                {
                    if (_client != null && _client.State != TcpSocketConnectionState.Closed || _clientFactory == null)
                        return _client;
                    return _client = _clientFactory();
                }
            }
        }

        public SimpleClientMessageSender(ITransportMessageEncoder transportMessageEncoder, Func<TcpSocketSaeaClient> clientFactory) : base(transportMessageEncoder)
        {
            _clientFactory = clientFactory;
        }

        public SimpleClientMessageSender(ITransportMessageEncoder transportMessageEncoder, TcpSocketSaeaClient client) : base(transportMessageEncoder)
        {
            _client = client;
        }

        #region Implementation of IMessageSender

        /// <summary>
        /// 发送消息。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <returns>一个任务。</returns>
        public async Task SendAsync(TransportMessage message)
        {
            var data = GetByteBuffer(message);
            await Client.SendAsync(data, 0, data.Length);
        }

        /// <summary>
        /// 发送消息并清空缓冲区。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <returns>一个任务。</returns>
        public async Task SendAndFlushAsync(TransportMessage message)
        {
            var data = GetByteBuffer(message);
            await Client.SendAsync(data, 0, data.Length);
        }

        #endregion Implementation of IMessageSender
    }

    public class SimpleServerMessageSender : SimpleMessageSender, IMessageSender
    {
        private readonly TcpSocketSaeaSession _session;

        public SimpleServerMessageSender(ITransportMessageEncoder transportMessageEncoder, TcpSocketSaeaSession session) : base(transportMessageEncoder)
        {
            _session = session;
        }

        #region Implementation of IMessageSender

        /// <summary>
        /// 发送消息。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <returns>一个任务。</returns>
        public async Task SendAsync(TransportMessage message)
        {
            //准备发送消息
            var data = GetByteBuffer(message);
            await _session.SendAsync(data, 0, data.Length);
        }

        /// <summary>
        /// 发送消息并清空缓冲区。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <returns>一个任务。</returns>
        public async Task SendAndFlushAsync(TransportMessage message)
        {
            //准备发送消息
            var data = GetByteBuffer(message);
            await _session.SendAsync(data, 0, data.Length);
        }

        #endregion Implementation of IMessageSender
    }
}
