using BodeAbp.Queue.Protocols;
using System.Collections.Generic;

namespace BodeAbp.Queue.Clients.Producers
{
    public interface IQueueSelector
    {
        int SelectQueueId(IList<int> availableQueueIds, Message message, string routingKey);
    }
}
