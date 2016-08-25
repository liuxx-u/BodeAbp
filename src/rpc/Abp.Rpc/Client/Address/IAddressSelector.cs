using System.Threading.Tasks;
using Abp.Rpc.Address;

namespace Abp.Rpc.Client.Address
{
    /// <summary>
    /// 一个抽象的地址选择器。
    /// </summary>
    public interface IAddressSelector
    {
        /// <summary>
        /// 选择一个地址。
        /// </summary>
        /// <param name="context">地址选择上下文。</param>
        /// <returns>地址模型。</returns>
        Task<AddressBase> SelectAsync(AddressSelectContext context);
    }
}
