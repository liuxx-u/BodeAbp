﻿namespace Abp.Net.Sockets
{
    public class SocketSetting
    {
        public int SendBufferSize = 1024 * 16;
        public int ReceiveBufferSize = 1024 * 16;

        public int MaxSendPacketSize = 1024 * 64;
        public int SendMessageFlowControlThreshold = 1000;
        public int SendMessageFlowControlStepPercent = 1;
        public int SendMessageFlowControlWaitMilliseconds = 1;

        public int ReceiveDataBufferSize = 1024 * 16;
        public int ReceiveDataBufferPoolSize = 50;
    }
}
