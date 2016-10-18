using System;
using BodeAbp.Queue.Protocols;

namespace BodeAbp.Queue.Protocols
{
    [Serializable]
    public class SendMessageRequest
    {
        public int QueueId { get; set; }
        public Message Message { get; set; }
    }
}
