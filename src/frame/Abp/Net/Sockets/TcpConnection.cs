﻿using Abp.Dependency;
using Abp.Extensions;
using Abp.Net.Sockets.Buffer;
using Abp.Net.Sockets.Framing;
using Castle.Core.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.Net.Sockets
{
    public interface ITcpConnection
    {
        bool IsConnected { get; }
        EndPoint LocalEndPoint { get; }
        EndPoint RemotingEndPoint { get; }
        void QueueMessage(byte[] message);
        void Close();
    }
    public class TcpConnection : ITcpConnection
    {
        #region Private Variables

        private readonly Socket _socket;
        private readonly SocketSetting _setting;
        private readonly EndPoint _localEndPoint;
        private readonly EndPoint _remotingEndPoint;
        private readonly SocketAsyncEventArgs _sendSocketArgs;
        private readonly SocketAsyncEventArgs _receiveSocketArgs;
        private readonly IBufferPool _receiveDataBufferPool;
        private readonly IMessageFramer _framer;
        private readonly ILogger _logger;
        private readonly ConcurrentQueue<IEnumerable<ArraySegment<byte>>> _sendingQueue = new ConcurrentQueue<IEnumerable<ArraySegment<byte>>>();
        private readonly ConcurrentQueue<ReceivedData> _receiveQueue = new ConcurrentQueue<ReceivedData>();
        private readonly MemoryStream _sendingStream = new MemoryStream();
        private readonly object _receivingLock = new object();

        private Action<ITcpConnection, SocketError> _connectionClosedHandler;
        private Action<ITcpConnection, byte[]> _messageArrivedHandler;

        private int _sending;
        private int _receiving;
        private int _parsing;
        private int _closing;

        private long _pendingMessageCount;

        #endregion

        #region Public Properties

        public bool IsConnected
        {
            get { return _socket.Connected; }
        }
        public Socket Socket
        {
            get { return _socket; }
        }
        public EndPoint LocalEndPoint
        {
            get { return _localEndPoint; }
        }
        public EndPoint RemotingEndPoint
        {
            get { return _remotingEndPoint; }
        }
        public SocketSetting Setting
        {
            get { return _setting; }
        }
        public long PendingMessageCount
        {
            get { return _pendingMessageCount; }
        }

        #endregion

        public TcpConnection(Socket socket, SocketSetting setting, IBufferPool receiveDataBufferPool, Action<ITcpConnection, byte[]> messageArrivedHandler, Action<ITcpConnection, SocketError> connectionClosedHandler)
        {
            socket.CheckNotNull("socket");
            setting.CheckNotNull("setting");
            receiveDataBufferPool.CheckNotNull("receiveDataBufferPool");
            messageArrivedHandler.CheckNotNull("messageArrivedHandler");
            connectionClosedHandler.CheckNotNull("connectionClosedHandler");

            _socket = socket;
            _setting = setting;
            _receiveDataBufferPool = receiveDataBufferPool;
            _localEndPoint = socket.LocalEndPoint;
            _remotingEndPoint = socket.RemoteEndPoint;
            _messageArrivedHandler = messageArrivedHandler;
            _connectionClosedHandler = connectionClosedHandler;

            _sendSocketArgs = new SocketAsyncEventArgs();
            _sendSocketArgs.AcceptSocket = _socket;
            _sendSocketArgs.Completed += OnSendAsyncCompleted;

            _receiveSocketArgs = new SocketAsyncEventArgs();
            _receiveSocketArgs.AcceptSocket = socket;
            _receiveSocketArgs.Completed += OnReceiveAsyncCompleted;

            _logger = IocManager.Instance.Resolve<ILoggerFactory>().Create(GetType().FullName);
            _framer = IocManager.Instance.Resolve<IMessageFramer>();
            _framer.RegisterMessageArrivedCallback(OnMessageArrived);

            TryReceive();
            TrySend();
        }

        public void QueueMessage(byte[] message)
        {
            if (message.Length == 0)
            {
                return;
            }

            var segments = _framer.FrameData(new ArraySegment<byte>(message, 0, message.Length));
            _sendingQueue.Enqueue(segments);
            Interlocked.Increment(ref _pendingMessageCount);

            TrySend();
        }
        public void Close()
        {
            CloseInternal(SocketError.Success, "Socket normal close.", null);
        }

        #region Send Methods

        private void TrySend()
        {
            if (_closing == 1) return;
            if (!EnterSending()) return;

            _sendingStream.SetLength(0);

            IEnumerable<ArraySegment<byte>> segments;
            while (_sendingQueue.TryDequeue(out segments))
            {
                Interlocked.Decrement(ref _pendingMessageCount);
                foreach (var segment in segments)
                {
                    _sendingStream.Write(segment.Array, segment.Offset, segment.Count);
                }
                if (_sendingStream.Length >= _setting.MaxSendPacketSize)
                {
                    break;
                }
            }

            if (_sendingStream.Length == 0)
            {
                ExitSending();
                if (_sendingQueue.Count > 0)
                {
                    TrySend();
                }
                return;
            }

            try
            {
                _sendSocketArgs.SetBuffer(_sendingStream.GetBuffer(), 0, (int)_sendingStream.Length);
                var firedAsync = _sendSocketArgs.AcceptSocket.SendAsync(_sendSocketArgs);
                if (!firedAsync)
                {
                    ProcessSend(_sendSocketArgs);
                }
            }
            catch (Exception ex)
            {
                CloseInternal(SocketError.Shutdown, "Socket send error, errorMessage:" + ex.Message, ex);
                ExitSending();
            }
        }
        private void ProcessSend(SocketAsyncEventArgs socketArgs)
        {
            if (_closing == 1) return;
            if (socketArgs.Buffer != null)
            {
                socketArgs.SetBuffer(null, 0, 0);
            }

            ExitSending();

            if (socketArgs.SocketError == SocketError.Success)
            {
                TrySend();
            }
            else
            {
                CloseInternal(socketArgs.SocketError, "Socket send error.", null);
            }
        }
        private void OnSendAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessSend(e);
        }
        private bool EnterSending()
        {
            return Interlocked.CompareExchange(ref _sending, 1, 0) == 0;
        }
        private void ExitSending()
        {
            Interlocked.Exchange(ref _sending, 0);
        }

        #endregion

        #region Receive Methods

        private void TryReceive()
        {
            if (!EnterReceiving()) return;

            var buffer = _receiveDataBufferPool.Get();
            if (buffer == null)
            {
                CloseInternal(SocketError.Shutdown, "Socket receive allocate buffer failed.", null);
                ExitReceiving();
                return;
            }

            _receiveSocketArgs.SetBuffer(buffer, 0, buffer.Length);
            if (_receiveSocketArgs.Buffer == null)
            {
                CloseInternal(SocketError.Shutdown, "Socket receive set buffer failed.", null);
                ExitReceiving();
                return;
            }

            try
            {
                bool firedAsync = _receiveSocketArgs.AcceptSocket.ReceiveAsync(_receiveSocketArgs);
                if (!firedAsync)
                {
                    ProcessReceive(_receiveSocketArgs);
                }
            }
            catch (Exception ex)
            {
                ReturnReceivingSocketBuffer();
                CloseInternal(SocketError.Shutdown, "Socket receive error, errorMessage:" + ex.Message, ex);
                ExitReceiving();
            }
        }
        private void OnReceiveAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceive(e);
        }
        private void ProcessReceive(SocketAsyncEventArgs socketArgs)
        {
            if (socketArgs.BytesTransferred == 0 || socketArgs.SocketError != SocketError.Success)
            {
                ReturnReceivingSocketBuffer();
                CloseInternal(socketArgs.SocketError, socketArgs.SocketError != SocketError.Success ? "Socket receive error" : "Socket normal close", null);
                return;
            }

            lock (_receivingLock)
            {
                var segment = new ArraySegment<byte>(socketArgs.Buffer, socketArgs.Offset, socketArgs.Count);
                _receiveQueue.Enqueue(new ReceivedData(segment, socketArgs.BytesTransferred));
                socketArgs.SetBuffer(null, 0, 0);
            }

            try
            {
                TryParsingReceivedData();
            }
            catch (Exception ex)
            {
                _logger.Error("Parsing received data error.", ex);
                ReturnReceivingSocketBuffer();
                CloseInternal(SocketError.Shutdown, "Parsing received data error.", ex);
                return;
            }

            ExitReceiving();
            TryReceive();
        }
        private void TryParsingReceivedData()
        {
            if (!EnterParsing()) return;

            try
            {
                var dataList = new List<ReceivedData>(_receiveQueue.Count);
                var segmentList = new List<ArraySegment<byte>>();

                ReceivedData data;
                while (_receiveQueue.TryDequeue(out data))
                {
                    dataList.Add(data);
                    segmentList.Add(new ArraySegment<byte>(data.Buf.Array, data.Buf.Offset, data.DataLen));
                }

                _framer.UnFrameData(segmentList);

                for (int i = 0, n = dataList.Count; i < n; ++i)
                {
                    _receiveDataBufferPool.Return(dataList[i].Buf.Array);
                }
            }
            finally
            {
                ExitParsing();
            }
        }
        private void OnMessageArrived(ArraySegment<byte> messageSegment)
        {
            byte[] message = new byte[messageSegment.Count];
            Array.Copy(messageSegment.Array, messageSegment.Offset, message, 0, messageSegment.Count);
            try
            {
                _messageArrivedHandler(this, message);
            }
            catch (Exception ex)
            {
                _logger.Error("Call message arrived handler failed.", ex);
            }
        }
        private void ReturnReceivingSocketBuffer()
        {
            try
            {
                if (_receiveSocketArgs.Buffer != null)
                {
                    _receiveDataBufferPool.Return(_receiveSocketArgs.Buffer);
                    _receiveSocketArgs.SetBuffer(null, 0, 0);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Return receiving socket event buffer failed.", ex);
            }
        }
        private bool EnterReceiving()
        {
            return Interlocked.CompareExchange(ref _receiving, 1, 0) == 0;
        }
        private void ExitReceiving()
        {
            Interlocked.Exchange(ref _receiving, 0);
        }
        private bool EnterParsing()
        {
            return Interlocked.CompareExchange(ref _parsing, 1, 0) == 0;
        }
        private void ExitParsing()
        {
            Interlocked.Exchange(ref _parsing, 0);
        }

        struct ReceivedData
        {
            public readonly ArraySegment<byte> Buf;
            public readonly int DataLen;

            public ReceivedData(ArraySegment<byte> buf, int dataLen)
            {
                Buf = buf;
                DataLen = dataLen;
            }
        }

        #endregion

        private void CloseInternal(SocketError socketError, string reason, Exception exception)
        {
            if (Interlocked.CompareExchange(ref _closing, 1, 0) == 0)
            {
                SocketUtils.ShutdownSocket(_socket);
                var isDisposedException = exception != null && exception is ObjectDisposedException;
                if (!isDisposedException)
                {
                    _logger.InfoFormat("Socket closed, remote endpoint:{0} socketError:{1}, reason:{2}", RemotingEndPoint, socketError, reason);
                }

                if (_connectionClosedHandler != null)
                {
                    try
                    {
                        _connectionClosedHandler(this, socketError);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Call connection closed handler failed.", ex);
                    }
                }
            }
        }
    }
}
