using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodeAbp.Zero.Providers
{
    internal class PermissionNames
    {
        //基础模块
        public const string Zero = "系统设置";

        //角色
        public const string Zero_Role = "角色";
        public const string Zero_Role_Crud = "角色管理";
        public const string Zero_Role_Test = "角色测试";

        //用户
        public const string Zero_User = "用户";
        public const string Zero_User_Crud = "用户管理";


        //审计日志
        public const string Zero_AuditLog = "审计日志";
        public const string Zero_AuditLog_Crud = "审计日志管理";
    }
}
