using BodeAbp.Queue.Protocols;

namespace BodeAbp.Queue.Clients.Consumers
{
    public class ConsumingMessage
    {
        public QueueMessage Message { get; private set; }
        public PullRequest PullRequest { get; private set; }
        public bool IsIgnored { get; set; }

        public ConsumingMessage(QueueMessage message, PullRequest pullRequest)
        {
            Message = message;
            PullRequest = pullRequest;
        }
    }
}
