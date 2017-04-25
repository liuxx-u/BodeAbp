using Abp.Extensions;
using Abp.Helper;

namespace Abp.Owin.Authentication
{
    /// <summary>
    /// 票据加密方式
    /// Base64(DES(ticket))+Base64(DES(_salt))
    /// </summary>
    internal class DesTicketInfoProtector : ITicketInfoProtector
    {
        public const string DefaultDesKey = "bode1234";
        public const string DefaultSalt = "http://user.cczcrv.com/";

        private readonly string _salt;
        private readonly string _desKey;

        public DesTicketInfoProtector(string desKey = DefaultDesKey, string salt = DefaultSalt)
        {
            _salt = salt;

            if (desKey.Length < 8)
            {
                throw new AbpException("key的长度必须大于8");
            }
            if (desKey.Length > 8 && desKey.Length < 24)
            {
                desKey = desKey.Left(8);
            }
            else if (desKey.Length > 24)
            {
                desKey = desKey.Left(24);
            }
            _desKey = desKey;
        }

        public string Protect(TicketInfo ticket)
        {
            ticket.CheckNotNull(nameof(ticket));

            var json = ticket.ToJsonString();
            return DesHelper.Encrypt(json, _desKey) + DesHelper.Encrypt(_salt, _desKey);
        }

        public TicketInfo UnProtect(string token)
        {
            token.CheckNotNullOrEmpty(nameof(token));

            var salt = DesHelper.Encrypt(_salt, _desKey);
            if (!token.EndsWith(salt)) return null;

            var json = token.Substring(0, token.Length - salt.Length);
            return DesHelper.Decrypt(json, _desKey).FromJsonString<TicketInfo>();
        }
    }
}