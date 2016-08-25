using System.Linq;
using System.Reflection;
using Abp.Extensions;

namespace Abp.Rpc.Ids
{
    /// <summary>
    /// 一个默认的服务Id生成器。
    /// </summary>
    public class DefaultServiceIdGenerator : IServiceIdGenerator
    {
        #region Implementation of IServiceIdFactory

        /// <summary>
        /// 生成一个服务Id。
        /// </summary>
        /// <param name="method">本地方法信息。</param>
        /// <returns>对应方法的唯一服务Id。</returns>
        public string GenerateServiceId(MethodInfo method)
        {
            method.CheckNotNull("method");
            var type = method.DeclaringType;
            type.CheckNotNull(string.Format("{0}方法的定义类型不能为空。", nameof(method.DeclaringType)));

            var id = $"{type.FullName}.{method.Name}";
            var parameters = method.GetParameters();
            if (parameters.Any())
            {
                id += "_" + string.Join("_", parameters.Select(i => i.Name));
            }
            return id;
        }

        #endregion Implementation of IServiceIdFactory
    }
}
