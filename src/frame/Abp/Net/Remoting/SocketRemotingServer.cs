﻿using Abp.Dependency;
using Abp.Net.Remoting.Args;
using Abp.Net.Sockets;
using Abp.Net.Sockets.Buffer;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Abp.Net.Remoting
{
    public class SocketRemotingServer
    {
        private readonly ServerSocket _serverSocket;
        private readonly Dictionary<int, IRequestHandler> _requestHandlerDict;
        private readonly IBufferPool _receiveDataBufferPool;
        private readonly ILogger _logger;
        private readonly SocketSetting _setting;
        private bool _isShuttingdown = false;

        public SocketRemotingServer() : this("Server", new IPEndPoint(SocketUtils.GetLocalIPV4(), 5000)) { }
        public SocketRemotingServer(string name, IPEndPoint listeningEndPoint, SocketSetting setting = null)
        {
            _setting = setting ?? new SocketSetting();
            _receiveDataBufferPool = new BufferPool(_setting.ReceiveDataBufferSize, _setting.ReceiveDataBufferPoolSize);
            _serverSocket = new ServerSocket(listeningEndPoint, _setting, _receiveDataBufferPool, HandleRemotingRequest);
            _requestHandlerDict = new Dictionary<int, IRequestHandler>();
            _logger = IocManager.Instance.Resolve<ILoggerFactory>().Create(name ?? GetType().Name);
        }

        public SocketRemotingServer RegisterConnectionEventListener(IConnectionEventListener listener)
        {
            _serverSocket.RegisterConnectionEventListener(listener);
            return this;
        }
        public SocketRemotingServer Start()
        {
            _isShuttingdown = false;
            _serverSocket.Start();
            return this;
        }
        public SocketRemotingServer Shutdown()
        {
            _isShuttingdown = true;
            _serverSocket.Shutdown();
            return this;
        }
        public SocketRemotingServer RegisterRequestHandler(int requestCode, IRequestHandler requestHandler)
        {
            _requestHandlerDict[requestCode] = requestHandler;
            return this;
        }

        private void HandleRemotingRequest(ITcpConnection connection, byte[] message, Action<byte[]> sendReplyAction)
        {
            if (_isShuttingdown) return;

            var remotingRequest = RemotingUtils.ParseRequest(message);
            var requestHandlerContext = new SocketRequestHandlerContext(connection, sendReplyAction);

            IRequestHandler requestHandler;
            if (!_requestHandlerDict.TryGetValue(remotingRequest.Code, out requestHandler))
            {
                var errorMessage = string.Format("No request handler found for remoting request:{0}", remotingRequest);
                _logger.Error(errorMessage);
                if (remotingRequest.Type != RemotingRequestType.Oneway)
                {
                    requestHandlerContext.SendRemotingResponse(new RemotingResponse(remotingRequest.Code, -1, remotingRequest.Type, Encoding.UTF8.GetBytes(errorMessage), remotingRequest.Sequence));
                }
                return;
            }

            try
            {
                var remotingResponse = requestHandler.HandleRequest(requestHandlerContext, remotingRequest);
                if (remotingRequest.Type != RemotingRequestType.Oneway && remotingResponse != null)
                {
                    requestHandlerContext.SendRemotingResponse(remotingResponse);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("Unknown exception raised when handling remoting request:{0}.", remotingRequest);
                _logger.Error(errorMessage, ex);
                if (remotingRequest.Type != RemotingRequestType.Oneway)
                {
                    requestHandlerContext.SendRemotingResponse(new RemotingResponse(remotingRequest.Code, -1, remotingRequest.Type, Encoding.UTF8.GetBytes(ex.Message), remotingRequest.Sequence));
                }
            }
        }
    }
}
