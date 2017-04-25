using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Abp.Extensions;

namespace Abp.Owin.Authentication
{
    public static class ClaimIdentityExtensions
    {
        public static ClaimsIdentity InitializeWithAllowHosts(this ClaimsIdentity identity,ICollection<string> hosts)
        {
            var exsitClaims = identity.FindAll(p => p.Type == SsoClaimTypes.AllowHosts);
            foreach (var claim in exsitClaims)
            {
                identity.RemoveClaim(claim);
            }

            if (hosts.Any())
            {
                identity.AddClaim(new Claim(SsoClaimTypes.AllowHosts, hosts.ExpandAndToString()));
            }

            return identity;
        }
    }
}