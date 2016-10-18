using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using System.Collections.Generic;

namespace Abp.Application.Services
{
    /// <summary>
    /// This class can be used as a base class for application services. 
    /// </summary>
    public abstract class ApplicationService : AbpServiceBase, IApplicationService, IAvoidDuplicateCrossCuttingConcerns
    {
        public static string[] CommonPostfixes = { "AppService", "ApplicationService" };

        /// <summary>
        /// Gets current session information.
        /// </summary>
        public IAbpSession AbpSession { get; set; }
        
        /// <summary>
        /// Reference to the permission manager.
        /// </summary>
        public IPermissionManager PermissionManager { protected get; set; }

        /// <summary>
        /// Reference to the permission checker.
        /// </summary>
        public IPermissionChecker PermissionChecker { protected get; set; }
        
        /// <summary>
        /// Gets the applied cross cutting concerns.
        /// </summary>
        public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ApplicationService()
        {
            AbpSession = NullAbpSession.Instance;
            PermissionChecker = NullPermissionChecker.Instance;
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        protected virtual Task<bool> IsGrantedAsync(string permissionName)
        {
            return PermissionChecker.IsGrantedAsync(permissionName);
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        protected virtual bool IsGranted(string permissionName)
        {
            return PermissionChecker.IsGranted(permissionName);
        }
    }
}
