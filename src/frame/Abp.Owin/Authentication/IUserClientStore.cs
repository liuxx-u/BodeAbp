using System.Collections.Generic;

namespace Abp.Owin.Authentication
{
    public interface IUserClientStore
    {
        ICollection<ClientInfo> GetUserClients(string userName);
    }
}
