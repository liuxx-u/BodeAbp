using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Rpc.Address;

namespace Abp.Rpc.Client.Address
{
    /// <summary>
    /// 地址选择器基类。
    /// </summary>
    public abstract class AddressSelectorBase : IAddressSelector
    {
        #region Implementation of IAddressSelector

        /// <summary>
        /// 选择一个地址。
        /// </summary>
        /// <param name="context">地址选择上下文。</param>
        /// <returns>地址模型。</returns>
        Task<AddressBase> IAddressSelector.SelectAsync(AddressSelectContext context)
        {
            context.CheckNotNull("context");
            context.Descriptor.CheckNotNull("context.Descriptor");
            context.Address.CheckNotNull("context.Address");

            var address = context.Address.ToArray();
            if (!address.Any())
                throw new ArgumentException("没有任何地址信息。", nameof(context.Address));

            return address.Length == 1 ? Task.FromResult(address[0]) : SelectAsync(context);
        }

        #endregion Implementation of IAddressSelector

        /// <summary>
        /// 选择一个地址。
        /// </summary>
        /// <param name="context">地址选择上下文。</param>
        /// <returns>地址模型。</returns>
        protected abstract Task<AddressBase> SelectAsync(AddressSelectContext context);
    }
}
