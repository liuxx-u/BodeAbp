﻿using BodeAbp.Queue.Protocols;
using System.Collections.Generic;

namespace BodeAbp.Queue.Broker
{
    public interface IConsumeOffsetStore
    {
        void Start();
        void Shutdown();
        int GetConsumerGroupCount();
        IEnumerable<string> GetAllConsumerGroupNames();
        bool DeleteConsumerGroup(string group);
        long GetConsumeOffset(string topic, int queueId, string group);
        long GetMinConsumedOffset(string topic, int queueId);
        void UpdateConsumeOffset(string topic, int queueId, long offset, string group);
        void DeleteConsumeOffset(QueueKey queueKey);
        void SetConsumeNextOffset(string topic, int queueId, string group, long nextOffset);
        bool TryFetchNextConsumeOffset(string topic, int queueId, string group, out long nextOffset);
        IEnumerable<QueueKey> GetConsumeKeys();
        IEnumerable<TopicConsumeInfo> QueryTopicConsumeInfos(string groupName, string topic);
    }
}
