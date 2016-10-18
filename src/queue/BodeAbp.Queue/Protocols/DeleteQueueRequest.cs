﻿using System;

namespace BodeAbp.Queue.Protocols
{
    [Serializable]
    public class DeleteQueueRequest
    {
        public string Topic { get; private set; }
        public int QueueId { get; private set; }

        public DeleteQueueRequest(string topic, int queueId)
        {
            Topic = topic;
            QueueId = queueId;
        }
    }
}
