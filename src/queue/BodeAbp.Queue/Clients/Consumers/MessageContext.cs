using BodeAbp.Queue.Protocols;
using System;

namespace BodeAbp.Queue.Clients.Consumers
{
    public class MessageContext : IMessageContext
    {
        public Action<QueueMessage> MessageHandledAction { get; private set; }

        public MessageContext(Action<QueueMessage> messageHandledAction)
        {
            MessageHandledAction = messageHandledAction;
        }

        public void OnMessageHandled(QueueMessage queueMessage)
        {
            MessageHandledAction(queueMessage);
        }
    }
}
