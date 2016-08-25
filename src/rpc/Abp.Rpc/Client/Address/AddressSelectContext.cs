using System.Collections.Generic;
using Abp.Rpc.Address;
using Abp.Rpc.Services;

namespace Abp.Rpc.Client.Address
{
    /// <summary>
    /// 地址选择上下文。
    /// </summary>
    public class AddressSelectContext
    {
        /// <summary>
        /// 服务描述符。
        /// </summary>
        public ServiceDescriptor Descriptor { get; set; }

        /// <summary>
        /// 服务可用地址。
        /// </summary>
        public IEnumerable<AddressBase> Address { get; set; }
    }
}
