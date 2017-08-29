using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Extensions;
using Microsoft.Owin.Security;

namespace Abp.Owin.Authentication
{
    public class SsoAuthenticationOptions : AuthenticationOptions
    {
        public const string DefaultCookieName = "sso.cookie";

        public string CookieName { get; set; }

        public string LoginPath { get; set; }

        public string ReturnUrlParameter { get; set; }

        private JavascriptCodeGenerator Javascript { get; }

        public ITicketInfoProtector TicketInfoProtector { get; set; }

        public ITicketInfoSessionStore SessionStore { get; set; }

        public IUserClientStore UserClientStore { get; set; }

        public SsoAuthenticationOptions() : base(DefaultCookieName)
        {
            CookieName = DefaultCookieName;
            ReturnUrlParameter = "ReturnUrl";
            Javascript = new JavascriptCodeGenerator();
        }

        public async Task<string> GetLoginJavascriptCode(ClaimsIdentity identity, string returnUrl)
        {
            identity.CheckNotNull(nameof(identity));
            UserClientStore.CheckNotNull(nameof(UserClientStore));

            var userClients = UserClientStore.GetUserClients(identity.Name);
            var allowHosts = userClients.Where(p => !p.Host.IsNullOrWhiteSpace()).Select(p => p.Host).ToList();
            identity = identity.InitializeWithAllowHosts(allowHosts);
            var token = await GenerateToken(identity);

            var loginNotifyUrls =
                userClients.Where(p => !p.LoginNotifyUrl.IsNullOrWhiteSpace()).Select(p => p.LoginNotifyUrl).ToList();
            return Javascript.GetLoginCode(token, loginNotifyUrls, returnUrl);
        }

        public string GetLogoutJavascriptCode(string userName)
        {
            UserClientStore.CheckNotNull(nameof(UserClientStore));

            var userClients = UserClientStore.GetUserClients(userName);
            var logoutNotifyUrls =
                userClients.Where(p => !p.LogoutNotifyUrl.IsNullOrWhiteSpace()).Select(p => p.LogoutNotifyUrl).ToList();
            return Javascript.GetLogoutCode(logoutNotifyUrls);
        }

        private async Task<string> GenerateToken(ClaimsIdentity identity)
        {
            var ticket = new TicketInfo(identity);
            if (SessionStore != null)
            {
                return await SessionStore.StoreAsync(ticket);
            }
            return TicketInfoProtector.Protect(ticket);
        }
    }
}