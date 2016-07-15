using System.Collections.Generic;
using Abp.Dependency;

namespace BodeAbp.Zero.Configuration
{
    public interface IRoleManagementConfig : ISingletonDependency
    {
        List<StaticRoleDefinition> StaticRoles { get; }
    }
}
