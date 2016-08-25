namespace BodeAbp.Zero.Permissions.Domain
{
    /// <summary>
    /// 授权信息
    /// </summary>
    public class PermissionGrantInfo
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 是否授权
        /// </summary>
        public bool IsGranted { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <param name="isGranted">是否授权</param>
        public PermissionGrantInfo(string name, bool isGranted)
        {
            Name = name;
            IsGranted = isGranted;
        }
    }
}
