using BodeAbp.Queue.Protocols;

namespace BodeAbp.Queue.Clients.Consumers
{
    public interface IMessageHandler
    {
        void Handle(QueueMessage message, IMessageContext context);
    }
}
