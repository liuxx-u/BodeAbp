﻿using System;
using System.IO;

namespace BodeAbp.Queue.Broker.Storage.LogRecords
{
    public class BufferLogRecord : ILogRecord
    {
        public byte[] RecordBuffer { get; set; }

        public void WriteTo(long logPosition, BinaryWriter writer)
        {
            writer.Write(RecordBuffer);
        }

        public void ReadFrom(byte[] recordBuffer)
        {
            RecordBuffer = new byte[4 + recordBuffer.Length];
            Buffer.BlockCopy(BitConverter.GetBytes(recordBuffer.Length), 0, RecordBuffer, 0, 4);
            Buffer.BlockCopy(recordBuffer, 0, RecordBuffer, 4, recordBuffer.Length);
        }
    }
}
