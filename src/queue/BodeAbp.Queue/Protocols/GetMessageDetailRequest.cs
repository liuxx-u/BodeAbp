using System;

namespace BodeAbp.Queue.Protocols
{
    [Serializable]
    public class GetMessageDetailRequest
    {
        public string MessageId { get; private set; }

        public GetMessageDetailRequest(string messageId)
        {
            MessageId = messageId;
        }

        public override string ToString()
        {
            return string.Format("[MessageId:{0}]", MessageId);
        }
    }
}
