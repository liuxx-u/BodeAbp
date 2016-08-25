using System.Text;
using Abp.Extensions;
using Abp.Rpc.Messages;

namespace Abp.Rpc.Transport.Codec
{
    public sealed class JsonTransportMessageDecoder : ITransportMessageDecoder
    {
        #region Implementation of ITransportMessageDecoder

        public TransportMessage Decode(byte[] data)
        {
            var content = Encoding.UTF8.GetString(data);
            var message = content.FromJsonString<TransportMessage>();
            if (message.IsInvokeMessage())
            {
                message.Content = message.Content.ToString().FromJsonString<RemoteInvokeMessage>();
            }
            if (message.IsInvokeResultMessage())
            {
                message.Content = message.Content.ToString().FromJsonString<RemoteInvokeResultMessage>();
            }
            return message;
        }

        #endregion Implementation of ITransportMessageDecoder
    }
}
