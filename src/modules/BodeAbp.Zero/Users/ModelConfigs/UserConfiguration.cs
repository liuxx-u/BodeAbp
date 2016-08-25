using Abp.EntityFramework;
using BodeAbp.Zero.Users.Domain;

namespace BodeAbp.Zero.EntityFramework.ModelConfigs
{
    public class UserConfiguration : EntityConfigurationBase<User, long>
    {
        public UserConfiguration()
        {
        }
    }
}
