using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BodeAbp.Zero.Navigations.Domain;
using BodeAbp.Zero.Navigations.Dtos;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Application.Services;
using System.Collections.Generic;

namespace BodeAbp.Zero.Navigations
{
    /// <summary>
    ///  导航信息 服务
    /// </summary>
    public class NavigationAppService : ApplicationService, INavigationAppService
    {
        /// <summary>
        /// 菜单 领域服务
        /// </summary>
        public NavigationManager navigationManager { protected get; set; }

        private readonly IRepository<Navigation> _navigationRepository;

        public NavigationAppService(IRepository<Navigation> navigationRepository)
        {
		    _navigationRepository = navigationRepository;
        }

		#region 导航信息

		 /// <inheritdoc/>
        public async Task<ICollection<NavigationInfo>> GetUserNavigations()
        {
            return await navigationManager.GetUserNavigations();
        }

        /// <inheritdoc/>
        public async Task<ICollection<NavigationInfo>> GetAllNavigations()
        {
            return await navigationManager.GetAllNavigations();
        }

        /// <inheritdoc/>
        public async Task CreateNavigation(NavigationInput input)
        {
            var navigation = input.MapTo<Navigation>();
            await navigationManager.CreateNavigationAsync(navigation);
        }

		 /// <inheritdoc/>
        public async Task UpdateNavigation(NavigationInput input)
        {
            var navigation = await _navigationRepository.GetAsync(input.Id);
            input.MapTo(navigation);
            await navigationManager.UpdateNavigationAsync(navigation);
        }

		/// <inheritdoc/>
        public async Task DeleteNavigation(IdInput input)
        {
            await _navigationRepository.DeleteAsync(input.Id);
        }

        /// <inheritdoc/>
        public async Task NavigationUp(IdInput input)
        {
            input.CheckNotNull("input");
            await navigationManager.NavigationUpAsync(input.Id);
        }

        /// <inheritdoc/>
        public async Task NavigationDown(IdInput input)
        {
            input.CheckNotNull("input");
            await navigationManager.NavigationDownAsync(input.Id);
        }

        #endregion
    }
}