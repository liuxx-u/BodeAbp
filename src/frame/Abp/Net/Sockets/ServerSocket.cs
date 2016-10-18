﻿using Abp.Dependency;
using Abp.Extensions;
using Abp.Net.Sockets.Buffer;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.Net.Sockets
{
    public class ServerSocket
    {
        #region Private Variables

        private readonly Socket _socket;
        private readonly SocketSetting _setting;
        private readonly IPEndPoint _listeningEndPoint;
        private readonly SocketAsyncEventArgs _acceptSocketArgs;
        private readonly IList<IConnectionEventListener> _connectionEventListeners;
        private readonly Action<ITcpConnection, byte[], Action<byte[]>> _messageArrivedHandler;
        private readonly IBufferPool _receiveDataBufferPool;
        private readonly ILogger _logger;

        #endregion

        public ServerSocket(IPEndPoint listeningEndPoint, SocketSetting setting, IBufferPool receiveDataBufferPool, Action<ITcpConnection, byte[], Action<byte[]>> messageArrivedHandler)
        {
            listeningEndPoint.CheckNotNull("listeningEndPoint");
            setting.CheckNotNull("setting");
            receiveDataBufferPool.CheckNotNull("receiveDataBufferPool");
            messageArrivedHandler.CheckNotNull("messageArrivedHandler");

            _listeningEndPoint = listeningEndPoint;
            _setting = setting;
            _receiveDataBufferPool = receiveDataBufferPool;
            _connectionEventListeners = new List<IConnectionEventListener>();
            _messageArrivedHandler = messageArrivedHandler;
            _socket = SocketUtils.CreateSocket(_setting.SendBufferSize, _setting.ReceiveBufferSize);
            _acceptSocketArgs = new SocketAsyncEventArgs();
            _acceptSocketArgs.Completed += AcceptCompleted;
            _logger = IocManager.Instance.Resolve<ILoggerFactory>().Create(GetType().FullName);
        }

        public void RegisterConnectionEventListener(IConnectionEventListener listener)
        {
            _connectionEventListeners.Add(listener);
        }
        public void Start()
        {
            _logger.InfoFormat("Socket server is starting, listening on TCP endpoint: {0}.", _listeningEndPoint);

            try
            {
                _socket.Bind(_listeningEndPoint);
                _socket.Listen(5000);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Failed to listen on TCP endpoint: {0}.", _listeningEndPoint), ex);
                SocketUtils.ShutdownSocket(_socket);
                throw;
            }

            StartAccepting();
        }
        public void Shutdown()
        {
            SocketUtils.ShutdownSocket(_socket);
            _logger.InfoFormat("Socket server shutdown, listening TCP endpoint: {0}.", _listeningEndPoint);
        }

        private void StartAccepting()
        {
            try
            {
                var firedAsync = _socket.AcceptAsync(_acceptSocketArgs);
                if (!firedAsync)
                {
                    ProcessAccept(_acceptSocketArgs);
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ObjectDisposedException))
                {
                    _logger.Info("Socket accept has exception, try to start accepting one second later.", ex);
                }
                Thread.Sleep(1000);
                StartAccepting();
            }
        }
        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success)
                {
                    var acceptSocket = e.AcceptSocket;
                    e.AcceptSocket = null;
                    OnSocketAccepted(acceptSocket);
                }
                else
                {
                    SocketUtils.ShutdownSocket(e.AcceptSocket);
                    e.AcceptSocket = null;
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                _logger.Error("Process socket accept has exception.", ex);
            }
            finally
            {
                StartAccepting();
            }
        }

        private void OnSocketAccepted(Socket socket)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var connection = new TcpConnection(socket, _setting, _receiveDataBufferPool, OnMessageArrived, OnConnectionClosed);

                    _logger.InfoFormat("Socket accepted, remote endpoint:{0}", socket.RemoteEndPoint);

                    foreach (var listener in _connectionEventListeners)
                    {
                        try
                        {
                            listener.OnConnectionAccepted(connection);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(string.Format("Notify connection accepted failed, listener type:{0}", listener.GetType().Name), ex);
                        }
                    }
                }
                catch (ObjectDisposedException) { }
                catch (Exception ex)
                {
                    _logger.Info("Accept socket client has unknown exception.", ex);
                }
            });
        }
        private void OnMessageArrived(ITcpConnection connection, byte[] message)
        {
            try
            {
                _messageArrivedHandler(connection, message, reply => connection.QueueMessage(reply));
            }
            catch (Exception ex)
            {
                _logger.Error("Handle message error.", ex);
            }
        }
        private void OnConnectionClosed(ITcpConnection connection, SocketError socketError)
        {
            foreach (var listener in _connectionEventListeners)
            {
                try
                {
                    listener.OnConnectionClosed(connection, socketError);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("Notify connection closed failed, listener type:{0}", listener.GetType().Name), ex);
                }
            }
        }
    }
}
