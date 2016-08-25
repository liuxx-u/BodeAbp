using Abp.Dependency;

namespace Abp.Authorization
{
    /// <summary>
    /// Permission dependency context.
    /// </summary>
    public interface IPermissionDependencyContext
    {
        /// <summary>
        /// The user which requires permission. Can be null if no user.
        /// </summary>
        UserIdentifier User { get; }

        /// <summary>
        /// Gets the <see cref="IIocResolver"/>.
        /// </summary>
        /// <value>
        /// The ioc resolver.
        /// </value>
        IIocResolver IocResolver { get; }
        
        /// <value>
        /// The permission checker.
        /// </value>
        IPermissionChecker PermissionChecker { get; }
    }
}