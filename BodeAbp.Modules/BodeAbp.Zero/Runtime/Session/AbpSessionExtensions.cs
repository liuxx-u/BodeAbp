using System;
using BodeAbp.Zero.Users.Domain;

namespace Abp.Runtime.Session
{
    public static class AbpSessionExtensions
    {
        public static bool IsUser(this IAbpSession session, User user)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return session.UserId.HasValue && 
                session.UserId.Value == user.Id;
        }
    }
}
