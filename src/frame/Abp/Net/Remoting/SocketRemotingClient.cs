﻿using Abp.Dependency;
using Abp.Extensions;
using Abp.Net.Remoting.Args;
using Abp.Net.Remoting.Exceptions;
using Abp.Schedule;
using Abp.Net.Sockets;
using Abp.Net.Sockets.Buffer;
using Castle.Core.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.Net.Remoting
{
    public class SocketRemotingClient
    {
        private readonly byte[] TimeoutMessage = Encoding.UTF8.GetBytes("Remoting request timeout.");
        private readonly Dictionary<int, IResponseHandler> _responseHandlerDict;
        private readonly IList<IConnectionEventListener> _connectionEventListeners;
        private readonly ConcurrentDictionary<long, ResponseFuture> _responseFutureDict;
        private readonly BlockingCollection<byte[]> _replyMessageQueue;
        private readonly IScheduleService _scheduleService;
        private readonly IBufferPool _receiveDataBufferPool;
        private readonly ILogger _logger;
        private readonly SocketSetting _setting;

        private EndPoint _serverEndPoint;
        private EndPoint _localEndPoint;
        private ClientSocket _clientSocket;
        private int _reconnecting = 0;
        private bool _shutteddown = false;
        private bool _started = false;

        public bool IsConnected
        {
            get { return _clientSocket.IsConnected; }
        }
        public EndPoint LocalEndPoint
        {
            get { return _localEndPoint; }
        }

        public SocketRemotingClient() : this(new IPEndPoint(SocketUtils.GetLocalIPV4(), 5000)) { }
        public SocketRemotingClient(EndPoint serverEndPoint, SocketSetting setting = null, EndPoint localEndPoint = null)
        {
            serverEndPoint.CheckNotNull("serverEndPoint");

            _serverEndPoint = serverEndPoint;
            _localEndPoint = localEndPoint;
            _setting = setting ?? new SocketSetting();
            _receiveDataBufferPool = new BufferPool(_setting.ReceiveDataBufferSize, _setting.ReceiveDataBufferPoolSize);
            _clientSocket = new ClientSocket(_serverEndPoint, _localEndPoint, _setting, _receiveDataBufferPool, HandleReplyMessage);
            _responseFutureDict = new ConcurrentDictionary<long, ResponseFuture>();
            _replyMessageQueue = new BlockingCollection<byte[]>(new ConcurrentQueue<byte[]>());
            _responseHandlerDict = new Dictionary<int, IResponseHandler>();
            _connectionEventListeners = new List<IConnectionEventListener>();
            _scheduleService = IocManager.Instance.Resolve<IScheduleService>();
            _logger = IocManager.Instance.Resolve<ILoggerFactory>().Create(GetType().FullName);

            RegisterConnectionEventListener(new ConnectionEventListener(this));
        }

        public SocketRemotingClient RegisterResponseHandler(int requestCode, IResponseHandler responseHandler)
        {
            _responseHandlerDict[requestCode] = responseHandler;
            return this;
        }
        public SocketRemotingClient RegisterConnectionEventListener(IConnectionEventListener listener)
        {
            _connectionEventListeners.Add(listener);
            _clientSocket.RegisterConnectionEventListener(listener);
            return this;
        }
        public SocketRemotingClient Start()
        {
            if (_started) return this;

            StartClientSocket();
            StartScanTimeoutRequestTask();
            _shutteddown = false;
            _started = true;
            return this;
        }
        public void Shutdown()
        {
            _shutteddown = true;
            StopReconnectServerTask();
            StopScanTimeoutRequestTask();
            ShutdownClientSocket();
        }

        public RemotingResponse InvokeSync(RemotingRequest request, int timeoutMillis)
        {
            var task = InvokeAsync(request, timeoutMillis);
            var response = task.WaitResult<RemotingResponse>(timeoutMillis + 1000);

            if (response == null)
            {
                if (!task.IsCompleted)
                {
                    throw new RemotingTimeoutException(_serverEndPoint, request, timeoutMillis);
                }
                else if (task.IsFaulted)
                {
                    throw new RemotingRequestException(_serverEndPoint, request, task.Exception);
                }
                else
                {
                    throw new RemotingRequestException(_serverEndPoint, request, "Remoting response is null due to unkown exception.");
                }
            }
            return response;
        }
        public Task<RemotingResponse> InvokeAsync(RemotingRequest request, int timeoutMillis)
        {
            EnsureClientStatus();

            request.Type = RemotingRequestType.Async;
            var taskCompletionSource = new TaskCompletionSource<RemotingResponse>();
            var responseFuture = new ResponseFuture(request, timeoutMillis, taskCompletionSource);

            if (!_responseFutureDict.TryAdd(request.Sequence, responseFuture))
            {
                throw new ResponseFutureAddFailedException(request.Sequence);
            }

            _clientSocket.QueueMessage(RemotingUtils.BuildRequestMessage(request));

            return taskCompletionSource.Task;
        }
        public void InvokeWithCallback(RemotingRequest request)
        {
            EnsureClientStatus();

            request.Type = RemotingRequestType.Callback;
            _clientSocket.QueueMessage(RemotingUtils.BuildRequestMessage(request));
        }
        public void InvokeOneway(RemotingRequest request)
        {
            EnsureClientStatus();

            request.Type = RemotingRequestType.Oneway;
            _clientSocket.QueueMessage(RemotingUtils.BuildRequestMessage(request));
        }

        private void HandleReplyMessage(ITcpConnection connection, byte[] message)
        {
            if (message == null) return;

            var remotingResponse = RemotingUtils.ParseResponse(message);

            if (remotingResponse.Type == RemotingRequestType.Callback)
            {
                IResponseHandler responseHandler;
                if (_responseHandlerDict.TryGetValue(remotingResponse.RequestCode, out responseHandler))
                {
                    responseHandler.HandleResponse(remotingResponse);
                }
                else
                {
                    _logger.ErrorFormat("No response handler found for remoting response:{0}", remotingResponse);
                }
            }
            else if (remotingResponse.Type == RemotingRequestType.Async)
            {
                ResponseFuture responseFuture;
                if (_responseFutureDict.TryRemove(remotingResponse.Sequence, out responseFuture))
                {
                    if (responseFuture.SetResponse(remotingResponse))
                    {
                        if (_logger.IsDebugEnabled)
                        {
                            _logger.DebugFormat("Remoting response back, request code:{0}, requect sequence:{1}, time spent:{2}", responseFuture.Request.Code, responseFuture.Request.Sequence, (DateTime.Now - responseFuture.BeginTime).TotalMilliseconds);
                        }
                    }
                    else
                    {
                        _logger.ErrorFormat("Set remoting response failed, response:" + remotingResponse);
                    }
                }
            }
        }
        private void ScanTimeoutRequest()
        {
            var timeoutKeyList = new List<long>();
            foreach (var entry in _responseFutureDict)
            {
                if (entry.Value.IsTimeout())
                {
                    timeoutKeyList.Add(entry.Key);
                }
            }
            foreach (var key in timeoutKeyList)
            {
                ResponseFuture responseFuture;
                if (_responseFutureDict.TryRemove(key, out responseFuture))
                {
                    responseFuture.SetResponse(new RemotingResponse(responseFuture.Request.Code, 0, responseFuture.Request.Type, TimeoutMessage, responseFuture.Request.Sequence));
                    if (_logger.IsDebugEnabled)
                    {
                        _logger.DebugFormat("Removed timeout request:{0}", responseFuture.Request);
                    }
                }
            }
        }
        private void ReconnectServer()
        {
            _logger.InfoFormat("Try to reconnect to server, address: {0}", _serverEndPoint);

            if (_clientSocket.IsConnected) return;
            if (!EnterReconnecting()) return;

            try
            {
                _clientSocket.Shutdown();
                _clientSocket = new ClientSocket(_serverEndPoint, _localEndPoint, _setting, _receiveDataBufferPool, HandleReplyMessage);
                foreach (var listener in _connectionEventListeners)
                {
                    _clientSocket.RegisterConnectionEventListener(listener);
                }
                _clientSocket.Start(2000);
            }
            catch (Exception ex)
            {
                _logger.Error("Reconnect to server error.", ex);
                ExitReconnecting();
            }
        }
        private void StartClientSocket()
        {
            _clientSocket.Start();
        }
        private void ShutdownClientSocket()
        {
            _clientSocket.Shutdown();
        }
        private void StartScanTimeoutRequestTask()
        {
            _scheduleService.StartTask(string.Format("{0}.ScanTimeoutRequest", this.GetType().Name), ScanTimeoutRequest, 1000, 1000);
        }
        private void StopScanTimeoutRequestTask()
        {
            _scheduleService.StopTask(string.Format("{0}.ScanTimeoutRequest", this.GetType().Name));
        }
        private void StartReconnectServerTask()
        {
            _scheduleService.StartTask(string.Format("{0}.ReconnectServer", this.GetType().Name), ReconnectServer, 1000, 1000);
        }
        private void StopReconnectServerTask()
        {
            _scheduleService.StopTask(string.Format("{0}.ReconnectServer", this.GetType().Name));
        }
        private void EnsureClientStatus()
        {
            if (!_clientSocket.IsConnected)
            {
                throw new RemotingServerUnAvailableException(_serverEndPoint);
            }
        }
        private bool EnterReconnecting()
        {
            return Interlocked.CompareExchange(ref _reconnecting, 1, 0) == 0;
        }
        private void ExitReconnecting()
        {
            Interlocked.Exchange(ref _reconnecting, 0);
        }
        private void SetLocalEndPoint(EndPoint localEndPoint)
        {
            _localEndPoint = localEndPoint;
        }

        class ConnectionEventListener : IConnectionEventListener
        {
            private readonly SocketRemotingClient _remotingClient;

            public ConnectionEventListener(SocketRemotingClient remotingClient)
            {
                _remotingClient = remotingClient;
            }

            public void OnConnectionAccepted(ITcpConnection connection) { }
            public void OnConnectionEstablished(ITcpConnection connection)
            {
                _remotingClient.StopReconnectServerTask();
                _remotingClient.ExitReconnecting();
                _remotingClient.SetLocalEndPoint(connection.LocalEndPoint);
            }
            public void OnConnectionFailed(SocketError socketError)
            {
                if (_remotingClient._shutteddown) return;

                _remotingClient.ExitReconnecting();
                _remotingClient.StartReconnectServerTask();
            }
            public void OnConnectionClosed(ITcpConnection connection, SocketError socketError)
            {
                if (_remotingClient._shutteddown) return;

                _remotingClient.ExitReconnecting();
                _remotingClient.StartReconnectServerTask();
            }
        }
    }
}
