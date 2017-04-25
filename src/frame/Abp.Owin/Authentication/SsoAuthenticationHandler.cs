using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace Abp.Owin.Authentication
{
    internal class SsoAuthenticationHandler : AuthenticationHandler<SsoAuthenticationOptions>
    {
        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            string requestCookie = Context.Request.Cookies[Options.CookieName];
            if (requestCookie.IsNullOrWhiteSpace()) return null;

            TicketInfo ticketInfo;

            if (Options.SessionStore != null)
            {
                ticketInfo = await Options.SessionStore.RetrieveAsync(requestCookie);
                if (!CheckAllowHost(ticketInfo)) return null;

                //如果超过一半的有效期，则刷新
                DateTime now = DateTime.Now;
                DateTime issuedTime = ticketInfo.LastRefreshTime ?? ticketInfo.CreationTime;
                DateTime expireTime = ticketInfo.ExpireTime;

                TimeSpan t1 = now - issuedTime;
                TimeSpan t2 = expireTime - now;
                if (t1 > t2)
                {
                    ticketInfo.LastRefreshTime = now;
                    ticketInfo.ExpireTime = now.Add(t1 + t2);

                    await Options.SessionStore.RenewAsync(requestCookie, ticketInfo);
                }
            }
            else
            {
                //未启用分布式存储器，需要前端定时请求刷新token
                ticketInfo = Options.TicketInfoProtector.UnProtect(requestCookie);
                if (!CheckAllowHost(ticketInfo)) return null;
            }

            if (ticketInfo != null && !ticketInfo.UserId.IsNullOrWhiteSpace())
            {
                var identity = ticketInfo.ToClaimsIdentity();
                AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                return ticket;
            }

            return null;
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode != 401 || Options.LoginPath.IsNullOrWhiteSpace())
            {
                return Task.FromResult(0);
            }
            var loginUrl = $"{Options.LoginPath}?{Options.ReturnUrlParameter}={Request.Uri}";
            Response.Redirect(loginUrl);

            return Task.FromResult<object>(null);
        }

        private bool CheckAllowHost(TicketInfo ticketInfo)
        {
            var claim = ticketInfo.Claims.FirstOrDefault(p => p.Name == SsoClaimTypes.AllowHosts);
            if (claim == null) return false;

            var allowHosts = claim.Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var currentHost = $"{Request.Scheme}://{Request.Host}/";
            return allowHosts.Contains(currentHost);
        }
    }
}