using BodeAbp.Queue.Protocols;
using System.Collections.Generic;

namespace BodeAbp.Queue.Clients.Consumers
{
    public interface IAllocateMessageQueueStrategy
    {
        IEnumerable<MessageQueue> Allocate(string currentConsumerId, IList<MessageQueue> totalMessageQueues, IList<string> totalConsumerIds);
    }
}
