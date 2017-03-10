using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BodeAbp.Zero.Navigations.Domain;
using BodeAbp.Zero.Navigations.Dtos;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Application.Services;
using System.Collections.Generic;
using Abp.Authorization;
using BodeAbp.Zero.Users.Domain;
using System.Linq;
using Abp.Auditing;

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
        private readonly IRepository<User, long> _userRepository;

        public NavigationAppService(IRepository<Navigation> navigationRepository, IRepository<User, long> userRepository)
        {
            _navigationRepository = navigationRepository;
            _userRepository = userRepository;
        }

        #region 导航信息

        /// <inheritdoc/>
        [AbpAuthorize]
        public async Task<ICollection<NavigationInfo>> GetUserNavigations()
        {
            var user = await _userRepository.GetAsync(AbpSession.UserId.Value);
            //return user.UserName == "admin"
            //    ? await navigationManager.GetAllNavigations()
            //    : await navigationManager.GetUserNavigations();
            return user.UserName == "admin"
                ? await navigationManager.GetAllNavigations()
                : await navigationManager.GetUserRoleNavigation();

        }

        /// <inheritdoc/>
        [DisableAuditing]
        public async Task<bool> CheckUserNavigation(NavigationCheckInput input)
        {
            if (AbpSession.UserId.HasValue)
            {
                var user = await _userRepository.GetAsync(AbpSession.UserId.Value);
                if (user.UserName == "admin") return true;
            }
            return await navigationManager.CheckUserNavigation(input.Url);
        }

        /// <inheritdoc/>
        public async Task<ICollection<NavigationInfo>> GetRoleAndMenu(int roleid)
        {
            return await navigationManager.GetRoleAndMenu(roleid);
        }

        /// <summary>
        /// 角色关联菜单
        /// </summary>
        /// <returns></returns>
        public async Task CreateRoleAndMenu(int roleid, string menuids)
        {
            await navigationManager.CreateRoleAndMenu(roleid, menuids);
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