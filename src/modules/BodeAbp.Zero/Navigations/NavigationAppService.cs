using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BodeAbp.Zero.Navigations.Domain;
using BodeAbp.Zero.Navigations.Dtos;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System;
using Abp.Extensions;

namespace BodeAbp.Zero.Navigations
{
    /// <summary>
    ///  导航信息 服务
    /// </summary>
    public class NavigationAppService : INavigationAppService
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
        public async Task<NavigationListOutput[]> GetAllNavigations()
        {
            var navigations = await _navigationRepository.GetAll().OrderBy(p => p.OrderNo).Select(p => new 
            {
                p.Id,
                p.Name,
                p.Url,
                p.Remark,
                p.Icon,
                p.ParentId,
                p.NavigationType
            }).ToListAsync();

            Func<int, NavigationListOutput[]> getChildren = null;
            getChildren = parentId =>
            {
                if (navigations.Any(m => m.ParentId == parentId))
                {
                    return navigations.Where(m => m.ParentId == parentId).Select(m => new NavigationListOutput()
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Url = m.Url,
                        Remark = m.Remark,
                        Icon = m.Icon,
                        ParentId = m.ParentId,
                        NavigationType = m.NavigationType,
                        Children = getChildren(m.Id)
                    }).ToArray();
                }
                return new NavigationListOutput[] { };
            };

            return getChildren(0);
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