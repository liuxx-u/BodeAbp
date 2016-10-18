﻿using System;

namespace BodeAbp.Queue.Protocols
{
    [Serializable]
    public class SetQueueConsumerVisibleRequest
    {
        public string Topic { get; private set; }
        public int QueueId { get; private set; }
        public bool Visible { get; private set; }

        public SetQueueConsumerVisibleRequest(string topic, int queueId, bool visible)
        {
            Topic = topic;
            QueueId = queueId;
            Visible = visible;
        }
    }
}
