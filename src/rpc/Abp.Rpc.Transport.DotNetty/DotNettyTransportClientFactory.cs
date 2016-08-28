using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Abp.Rpc.Messages;
using Abp.Rpc.Server;
using Abp.Rpc.Transport.Codec;
using Abp.Rpc.Transport.DotNetty.Adaper;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Abp.Rpc.Transport.DotNetty
{
    /// <summary>
    /// 基于DotNetty的传输客户端工厂。
    /// </summary>
    public class DotNettyTransportClientFactory : ITransportClientFactory, IDisposable
    {
        #region Field

        private readonly ITransportMessageEncoder _transportMessageEncoder;
        private readonly ITransportMessageDecoder _transportMessageDecoder;
        private readonly IServiceExecutor _serviceExecutor;
        private readonly ConcurrentDictionary<string, Lazy<ITransportClient>> _clients = new ConcurrentDictionary<string, Lazy<ITransportClient>>();
        private readonly Bootstrap _bootstrap;

        #endregion Field

        #region Constructor

        public DotNettyTransportClientFactory(ITransportMessageCodecFactory codecFactory)
            : this(codecFactory, null)
        {
        }

        public DotNettyTransportClientFactory(ITransportMessageCodecFactory codecFactory, IServiceExecutor serviceExecutor)
        {
            _transportMessageEncoder = codecFactory.GetEncoder();
            _transportMessageDecoder = codecFactory.GetDecoder();
            _serviceExecutor = serviceExecutor;
            _bootstrap = GetBootstrap();
        }

        #endregion Constructor

        #region Implementation of ITransportClientFactory

        /// <summary>
        /// 创建客户端。
        /// </summary>
        /// <param name="endPoint">终结点。</param>
        /// <returns>传输客户端实例。</returns>
        public ITransportClient CreateClient(EndPoint endPoint)
        {
            var key = endPoint.ToString();
            //$"准备为服务端地址：{key}创建客户端。"
            return _clients.GetOrAdd(key
                , k => new Lazy<ITransportClient>(() =>
                {
                    var messageListener = new MessageListener();

                    _bootstrap.Handler(new ActionChannelInitializer<ISocketChannel>(c =>
                    {
                        var pipeline = c.Pipeline;
                        pipeline.AddLast(new LengthFieldPrepender(4));
                        pipeline.AddLast(new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                        pipeline.AddLast(new TransportMessageChannelHandlerAdapter(_transportMessageDecoder));
                        pipeline.AddLast(new DefaultChannelHandler(messageListener, _transportMessageEncoder));
                    }));

                    var bootstrap = _bootstrap;
                    var channel = bootstrap.ConnectAsync(endPoint);
                    var messageSender = new DotNettyMessageClientSender(_transportMessageEncoder, channel);
                    var client = new TransportClient(messageSender, messageListener, _serviceExecutor);
                    return client;
                }
                )).Value;
        }

        #endregion Implementation of ITransportClientFactory

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            foreach (var client in _clients.Values.Where(i => i.IsValueCreated))
            {
                (client.Value as IDisposable)?.Dispose();
            }
        }

        #endregion Implementation of IDisposable

        private static Bootstrap GetBootstrap()
        {
            var bootstrap = new Bootstrap();
            bootstrap
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Group(new MultithreadEventLoopGroup());

            return bootstrap;
        }

        protected class DefaultChannelHandler : ChannelHandlerAdapter
        {
            private readonly IMessageListener _messageListener;
            private readonly ITransportMessageEncoder _transportMessageEncoder;

            public DefaultChannelHandler(IMessageListener messageListener, ITransportMessageEncoder transportMessageEncoder)
            {
                _messageListener = messageListener;
                _transportMessageEncoder = transportMessageEncoder;
            }

            #region Overrides of ChannelHandlerAdapter

            public override void ChannelRead(IChannelHandlerContext context, object message)
            {
                var transportMessage = message as TransportMessage;

                _messageListener.OnReceived(new DotNettyMessageClientSender(_transportMessageEncoder, Task.FromResult(context.Channel)), transportMessage);
            }

            #endregion Overrides of ChannelHandlerAdapter
        }
    }
}