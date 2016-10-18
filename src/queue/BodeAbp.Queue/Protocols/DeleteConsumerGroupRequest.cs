using System;

namespace BodeAbp.Queue.Protocols
{
    [Serializable]
    public class DeleteConsumerGroupRequest
    {
        public string GroupName { get; private set; }

        public DeleteConsumerGroupRequest(string groupName)
        {
            GroupName = groupName;
        }

        public override string ToString()
        {
            return string.Format("[GroupName:{0}]", GroupName);
        }
    }
}
