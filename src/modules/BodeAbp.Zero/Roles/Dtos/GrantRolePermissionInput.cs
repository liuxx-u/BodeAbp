using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace BodeAbp.Zero.Roles.Dtos
{
    public class GrantRolePermissionInput : IInputDto
    {
        public GrantRolePermissionInput()
        {
            PermissionNames = new List<string>();
        }

        public int RoleId { get; set; }

        public ICollection<string> PermissionNames { get; set; }
    }
}
