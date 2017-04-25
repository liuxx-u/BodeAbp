using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace Abp.Owin.Authentication
{
    public class SsoAuthenticationOptions: AuthenticationOptions
    {
        public const string DefaultCookieName = "sso.cookie";

        public string CookieName { get; set; }

        public string LoginPath { get; set; }

        public string ReturnUrlParameter { get; set; }

        public JavascriptCodeGenerator Javascript { get; set; }

        public ITicketInfoProtector TicketInfoProtector { get; set; }

        public ITicketInfoSessionStore SessionStore { get; set; }

        public SsoAuthenticationOptions() : base(DefaultCookieName)
        {
            CookieName = DefaultCookieName;
            ReturnUrlParameter = "ReturnUrl";
            Javascript = new JavascriptCodeGenerator();
        }

        public async Task<string> GenerateToken(ClaimsIdentity identity)
        {
            var tiketInfo = new TicketInfo(identity);
            if (SessionStore != null)
            {
                return await SessionStore.StoreAsync(tiketInfo); 
            }
            return TicketInfoProtector.Protect(tiketInfo);
        }
    }
}