namespace BodeAbp.Zero.Configuration
{
    public class StaticRoleDefinition
    {
        public string RoleName { get; private set; }

        public StaticRoleDefinition(string roleName)
        {
            RoleName = roleName;
        }
    }
}
