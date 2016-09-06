using Abp.Dependency;
using System;
using System.Collections.Generic;

namespace Abp.Net.Sockets.Framing
{
    public interface IMessageFramer : ITransientDependency
    {
        void UnFrameData(IEnumerable<ArraySegment<byte>> data);
        void UnFrameData(ArraySegment<byte> data);
        IEnumerable<ArraySegment<byte>> FrameData(ArraySegment<byte> data);
        void RegisterMessageArrivedCallback(Action<ArraySegment<byte>> handler);
    }
}
