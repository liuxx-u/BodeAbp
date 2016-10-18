using BodeAbp.Queue.Protocols;

namespace BodeAbp.Queue.Clients.Consumers
{
    public interface IMessageContext
    {
        void OnMessageHandled(QueueMessage queueMessage);
    }
}
