using System;

namespace BodeAbp.Queue.Protocols
{
    [Serializable]
    public class AddQueueRequest
    {
        public string Topic { get; private set; }

        public AddQueueRequest(string topic)
        {
            Topic = topic;
        }
    }
}
