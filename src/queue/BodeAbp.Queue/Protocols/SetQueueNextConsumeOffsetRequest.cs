﻿using System;

namespace BodeAbp.Queue.Protocols
{
    [Serializable]
    public class SetQueueNextConsumeOffsetRequest
    {
        public string ConsumerGroup { get; set; }
        public string Topic { get; private set; }
        public int QueueId { get; private set; }
        public long NextOffset { get; set; }

        public SetQueueNextConsumeOffsetRequest(string consumerGroup, string topic, int queueId, long nextOffset)
        {
            ConsumerGroup = consumerGroup;
            Topic = topic;
            QueueId = queueId;
            NextOffset = nextOffset;
        }

        public override string ToString()
        {
            return string.Format("[ConsumerGroup:{0}, Topic:{1}, QueueId:{2}, NextOffset:{3}]", ConsumerGroup, Topic, QueueId, NextOffset);
        }
    }
}
