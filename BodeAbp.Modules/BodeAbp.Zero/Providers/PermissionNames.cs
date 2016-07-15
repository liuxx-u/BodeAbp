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
        public const string Zero = "Zero";

        //角色
        public const string Zero_Role = "Zero.Role";
        public const string Zero_Role_Crud = "Zero.Role.Crud";
        public const string Zero_Role_Test = "Zero.Role.Test";

        //用户
        public const string Zero_User = "Zero.User";
        public const string Zero_User_Crud = "Zero.User.Crud";


        //审计日志
        public const string Zero_AuditLog = "Zero.AuditLog";
        public const string Zero_AuditLog_Crud = "Zero.AuditLog.Crud";
    }
}
