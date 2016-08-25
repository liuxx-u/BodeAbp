using System.Text;
using Abp.Extensions;
using Abp.Rpc.Messages;

namespace Abp.Rpc.Transport.Codec
{
    public sealed class JsonTransportMessageEncoder : ITransportMessageEncoder
    {
        #region Implementation of ITransportMessageEncoder

        public byte[] Encode(TransportMessage message)
        {
            var content = message.ToJsonString();
            return Encoding.UTF8.GetBytes(content);
        }

        #endregion Implementation of ITransportMessageEncoder
    }
}
