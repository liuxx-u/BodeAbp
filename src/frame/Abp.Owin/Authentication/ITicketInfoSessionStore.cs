using System.Threading.Tasks;

namespace Abp.Owin.Authentication
{
    /// <summary>
    /// 票据共享存储器
    /// </summary>
    public interface ITicketInfoSessionStore
    {
        Task<string> StoreAsync(TicketInfo ticket);

        Task RenewAsync(string key, TicketInfo ticket);

        Task<TicketInfo> RetrieveAsync(string key);

        Task RemoveAsync(string key);
    }
}