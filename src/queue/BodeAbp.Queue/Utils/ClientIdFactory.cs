using Abp.Extensions;
using System.Net;

namespace BodeAbp.Queue.Utils
{
    public static class ClientIdFactory
    {
        public static string CreateClientId(IPEndPoint clientEndPoint)
        {
            clientEndPoint.CheckNotNull("clientEndPoint");
            return string.Format("{0}@{1}", clientEndPoint.Address, clientEndPoint.Port);
        }
    }
}
