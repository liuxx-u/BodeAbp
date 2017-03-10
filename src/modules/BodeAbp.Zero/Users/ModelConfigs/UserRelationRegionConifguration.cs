using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using BodeAbp.Zero.Users.Domain;

namespace BodeAbp.Zero.Users.ModelConfigs
{
    class UserRelationRegionConifguration : EntityConfigurationBase<UserRelationRegion, long>
    {
        public UserRelationRegionConifguration()
        {
        }
    }
}
