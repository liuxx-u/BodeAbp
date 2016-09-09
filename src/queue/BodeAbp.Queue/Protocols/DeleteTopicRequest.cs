using System;

namespace BodeAbp.Queue.Protocols
{
    [Serializable]
    public class DeleteTopicRequest
    {
        public string Topic { get; private set; }

        public DeleteTopicRequest(string topic)
        {
            Topic = topic;
        }
    }
}
