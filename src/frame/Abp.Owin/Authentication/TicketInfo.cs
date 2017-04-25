using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Abp.Owin.Authentication
{
    public class TicketInfo
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string AuthenticationType { get; set; } = "sso.cookie";

        public DateTime CreationTime { get; set; }

        public DateTime? LastRefreshTime { get; set; }

        public DateTime ExpireTime { get; set; }

        public List<NameValue> Claims { get; set; }

        public TicketInfo()
        {
            CreationTime = DateTime.Now;
            ExpireTime = CreationTime.AddHours(2);//默认有效期：2小时
            Claims = new List<NameValue>();
        }

        public TicketInfo(ClaimsIdentity identity) : this()
        {
            UserId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Name = identity.Name;

            Claims = identity.Claims.Select(p => new NameValue(p.Type, p.Value)).ToList();
        }

        public ClaimsIdentity ToClaimsIdentity()
        {
            var claims = Claims.Select(p => new Claim(p.Name, p.Value));
            var identity = new ClaimsIdentity(claims, AuthenticationType);
            return identity;
        }
    }
}