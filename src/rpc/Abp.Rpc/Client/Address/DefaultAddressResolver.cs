using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Rpc.Address;
using Abp.Rpc.Client.HealthChecks;
using Abp.Rpc.Routing;

namespace Abp.Rpc.Client.Address
{
    /// <summary>
    /// 一个默认的服务地址解析器。
    /// </summary>
    public class DefaultAddressResolver : IAddressResolver
    {
        #region Field

        private readonly IServiceRouteManager _serviceRouteManager;
        private readonly IAddressSelector _addressSelector;
        private readonly IHealthCheckService _healthCheckService;

        #endregion Field

        #region Constructor

        public DefaultAddressResolver(IServiceRouteManager serviceRouteManager, IAddressSelector addressSelector, IHealthCheckService healthCheckService)
        {
            _serviceRouteManager = serviceRouteManager;
            _addressSelector = addressSelector;
            _healthCheckService = healthCheckService;
        }

        #endregion Constructor

        #region Implementation of IAddressResolver

        /// <summary>
        /// 解析服务地址。
        /// </summary>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>服务地址模型。</returns>
        public async Task<AddressBase> Resolver(string serviceId)
        {
            //$"准备为服务id：{serviceId}，解析可用地址。"
            var descriptors = await _serviceRouteManager.GetRoutesAsync();
            var descriptor = descriptors.FirstOrDefault(i => i.ServiceDescriptor.Id == serviceId);

            if (descriptor == null)
            {
                //$"根据服务id：{serviceId}，找不到相关服务信息。"
                return null;
            }

            var address = new List<AddressBase>();
            foreach (var addressModel in descriptor.Address)
            {
                await _healthCheckService.Monitor(addressModel);
                if (!await _healthCheckService.IsHealth(addressModel))
                    continue;

                address.Add(addressModel);
            }

            var hasAddress = address.Any();
            if (!hasAddress)
            {
                //$"根据服务id：{serviceId}，找不到可用的地址。"
                return null;
            }

            //$"根据服务id：{serviceId}，找到以下可用地址：{string.Join(",", address.Select(i => i.ToString()))}。"
            return await _addressSelector.SelectAsync(new AddressSelectContext
            {
                Descriptor = descriptor.ServiceDescriptor,
                Address = address
            });
        }

        #endregion Implementation of IAddressResolver
    }
}
