﻿using Abp.Application.Services.Dto;

namespace BodeAbp.Zero.Users.Dtos
{
    public class UserRoleSelectOutPut
    {
        public string RoleName { get; set; }

        public string DisplayName { get; set; }

        public bool IsChecked { get; set; }
    }
}
