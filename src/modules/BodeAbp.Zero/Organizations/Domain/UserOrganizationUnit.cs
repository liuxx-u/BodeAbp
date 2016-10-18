﻿using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace BodeAbp.Zero.Organizations.Domain
{
    /// <summary>
    /// Represents membership of a User to an OU.
    /// </summary>
    [Table("Zero_UserOrganizationUnit")]
    public class UserOrganizationUnit : CreationAuditedEntity<long>
    {
        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Id of the <see cref="OrganizationUnit"/>.
        /// </summary>
        public virtual long OrganizationUnitId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOrganizationUnit"/> class.
        /// </summary>
        public UserOrganizationUnit()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOrganizationUnit"/> class.
        /// </summary>
        /// <param name="tenantId">TenantId</param>
        /// <param name="userId">Id of the User.</param>
        /// <param name="organizationUnitId">Id of the <see cref="OrganizationUnit"/>.</param>
        public UserOrganizationUnit(long userId, long organizationUnitId)
        {
            UserId = userId;
            OrganizationUnitId = organizationUnitId;
        }
    }
}
