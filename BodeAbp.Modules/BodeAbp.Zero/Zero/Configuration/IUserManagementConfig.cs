using Abp.Collections;
using Abp.Dependency;

namespace BodeAbp.Zero.Configuration
{
    /// <summary>
    /// User management configuration.
    /// </summary>
    public interface IUserManagementConfig : ISingletonDependency
    {
        ITypeList<object> ExternalAuthenticationSources { get; set; }
    }
}
