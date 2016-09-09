using Abp;

namespace EQueue.Broker.Exceptions
{
    public class QueueNotExistException : AbpException
    {
        public QueueNotExistException(string topic, int queueId)
            : base(string.Format("Queue not exist. topic:{0}, queueId:{1}", topic, queueId))
        {
        }
    }
}
