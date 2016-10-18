using Abp.Dependency;
using Abp.Net.Remoting;
using Abp.Net.Remoting.Args;
using Abp.Serialization;
using BodeAbp.Queue.Protocols;
using BodeAbp.Queue.Utils;
using EQueue.Broker.Exceptions;
using System.Collections.Generic;

namespace BodeAbp.Queue.Broker.RequestHandlers.Admin
{
    public class GetMessageDetailRequestHandler : IRequestHandler
    {
        private readonly IMessageStore _messageStore;
        private ISerializer<byte[]> _binarySerializer;

        public GetMessageDetailRequestHandler()
        {
            _messageStore = IocManager.Instance.Resolve<IMessageStore>();
            _binarySerializer = IocManager.Instance.Resolve<ISerializer<byte[]>>();
        }

        public RemotingResponse HandleRequest(IRequestHandlerContext context, RemotingRequest remotingRequest)
        {
            if (BrokerController.Instance.IsCleaning)
            {
                throw new BrokerCleanningException();
            }
            var request = _binarySerializer.Deserialize<byte[],GetMessageDetailRequest>(remotingRequest.Body);
            var messageInfo = MessageIdUtil.ParseMessageId(request.MessageId);
            var message = _messageStore.GetMessage(messageInfo.MessagePosition);
            var messages = new List<QueueMessage>();
            if (message != null)
            {
                messages.Add(message);
            }
            return RemotingResponseFactory.CreateResponse(remotingRequest, _binarySerializer.Serialize(messages));
        }
    }
}
