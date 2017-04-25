using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Runtime.Caching;

namespace Abp.Owin.Authentication
{
    public class DefaultTicketInfoSessionStore : ITicketInfoSessionStore
    {
        public const string SsoCacheKey = "sso.token";

        private readonly ICacheManager _cacheManager;
        private readonly ITicketInfoProtector _ticketInfoProtector;
        public DefaultTicketInfoSessionStore(ITicketInfoProtector ticketInfoProtector)
        {
            _ticketInfoProtector = ticketInfoProtector;

            //ICacheManager需要部署为分布式缓存
            _cacheManager = IocManager.Instance.Resolve<ICacheManager>();
        }

        public async Task RemoveAsync(string key)
        {
            await _cacheManager.GetCache(SsoCacheKey).RemoveAsync(key);
        }

        public async Task RenewAsync(string key, TicketInfo ticket)
        {
            var token = _ticketInfoProtector.Protect(ticket);
            await _cacheManager.GetCache(SsoCacheKey).SetAsync(key, token);
        }

        public async Task<TicketInfo> RetrieveAsync(string key)
        {
            var token = await _cacheManager.GetCache<string, string>(SsoCacheKey).GetOrDefaultAsync(key);
            return _ticketInfoProtector.UnProtect(token);
        }

        public async Task<string> StoreAsync(TicketInfo ticket)
        {
            var key = new Guid().ToString();
            var token = _ticketInfoProtector.Protect(ticket);
            await _cacheManager.GetCache(SsoCacheKey).SetAsync(key, token);
            return key;
        }
    }
}