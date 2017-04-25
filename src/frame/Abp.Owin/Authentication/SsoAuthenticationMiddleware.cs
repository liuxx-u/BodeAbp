using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace Abp.Owin.Authentication
{
    public class SsoAuthenticationMiddleware : AuthenticationMiddleware<SsoAuthenticationOptions>
    {
        public SsoAuthenticationMiddleware(OwinMiddleware next, SsoAuthenticationOptions options) : base(next, options)
        {
            if (string.IsNullOrEmpty(Options.CookieName))
            {
                Options.CookieName = SsoAuthenticationOptions.DefaultCookieName;
            }
            if (Options.TicketInfoProtector == null)
            {
                Options.TicketInfoProtector = new DesTicketInfoProtector();
            }
        }

        protected override AuthenticationHandler<SsoAuthenticationOptions> CreateHandler()
        {
            return new SsoAuthenticationHandler();
        }
    }
}