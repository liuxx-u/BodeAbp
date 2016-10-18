﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Abp.Serialization
{
    /// <summary>
    /// 基于string类型的byte[]序列化器。
    /// </summary>
    public class BinarySerializer : ISerializer<byte[]>
    {
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

        /// <summary>Serialize an object to byte array.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                _binaryFormatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }
        /// <summary>Deserialize an object from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Deserialize(byte[] data, Type type)
        {
            using (var stream = new MemoryStream(data))
            {
                return _binaryFormatter.Deserialize(stream);
            }
        }
        /// <summary>Deserialize a typed object from a byte array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] data) where T : class
        {
            using (var stream = new MemoryStream(data))
            {
                return _binaryFormatter.Deserialize(stream) as T;
            }
        }
    }
}