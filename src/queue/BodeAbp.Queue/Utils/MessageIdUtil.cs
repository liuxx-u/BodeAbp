﻿using Abp;
using BodeAbp.Queue.Broker;
using System;
using System.Linq;
using System.Net;

namespace BodeAbp.Queue.Utils
{
    public class MessageIdUtil
    {
        private static byte[] _ipBytes;
        private static byte[] _portBytes;

        public static string CreateMessageId(long messagePosition)
        {
            if (_ipBytes == null)
            {
                _ipBytes = BrokerController.Instance.Setting.ProducerAddress.Address.GetAddressBytes();
            }
            if (_portBytes == null)
            {
                _portBytes = BitConverter.GetBytes(BrokerController.Instance.Setting.ProducerAddress.Port);
            }
            var positionBytes = BitConverter.GetBytes(messagePosition);
            var messageIdBytes = Combine(_ipBytes, _portBytes, positionBytes);

            return ObjectId.ToHexString(messageIdBytes);
        }
        public static MessageIdInfo ParseMessageId(string messageId)
        {
            var messageIdBytes = ObjectId.ParseHexString(messageId);
            var ipBytes = new byte[4];
            var portBytes = new byte[4];
            var messagePositionBytes = new byte[8];

            Buffer.BlockCopy(messageIdBytes, 0, ipBytes, 0, 4);
            Buffer.BlockCopy(messageIdBytes, 4, portBytes, 0, 4);
            Buffer.BlockCopy(messageIdBytes, 8, messagePositionBytes, 0, 8);

            var ip = BitConverter.ToInt32(ipBytes, 0);
            var port = BitConverter.ToInt32(portBytes, 0);
            var messagePosition = BitConverter.ToInt64(messagePositionBytes, 0);

            return new MessageIdInfo
            {
                IP = new IPAddress(ip),
                Port = port,
                MessagePosition = messagePosition
            };
        }

        private static byte[] Combine(params byte[][] arrays)
        {
            byte[] destination = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, destination, offset, data.Length);
                offset += data.Length;
            }
            return destination;
        }
    }
    public struct MessageIdInfo
    {
        public IPAddress IP;
        public int Port;
        public long MessagePosition;
    }
}
