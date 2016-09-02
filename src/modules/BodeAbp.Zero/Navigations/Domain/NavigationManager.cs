using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using BodeAbp.Zero.Roles.Domain;

namespace BodeAbp.Zero.Navigations.Domain
{
    /// <summary>
    /// 导航菜单 领域服务
    /// </summary>
    public class NavigationManager : IDomainService
    {
        /// <summary>
        /// Gets current session information.
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Navigation> _navigationRepository;
        private readonly IRepository<RoleNavigation> _roleNavigationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="navigationRepository"></param>
        /// <param name="roleNavigationRepository"></param>
        public NavigationManager(IRepository<Navigation> navigationRepository, IRepository<RoleNavigation> roleNavigationRepository)
        {
            _navigationRepository = navigationRepository;
            _roleNavigationRepository = roleNavigationRepository;
        }

        /// <summary>
        /// 创建  菜单
        /// </summary>
        /// <param name="navigation">菜单</param>
        /// <returns></returns>
        public async Task CreateNavigationAsync(Navigation navigation)
        {
            await CheckNavigationAsync(navigation);
            var orderNos = _navigationRepository.GetAll().Where(p => p.ParentId == navigation.ParentId).Select(p => p.OrderNo).ToArray();
            navigation.OrderNo = orderNos.Any() ? orderNos.Max() + 1 : 1;
            await _navigationRepository.InsertAsync(navigation);
        }

        /// <summary>
        /// 修改  菜单
        /// </summary>
        /// <param name="navigation">菜单</param>
        /// <returns></returns>
        public async Task UpdateNavigationAsync(Navigation navigation)
        {
            await CheckNavigationAsync(navigation);
            await _navigationRepository.UpdateAsync(navigation);
        }


        /// <summary>
        /// 菜单升序
        /// </summary>
        /// <param name="navigationId">菜单Id</param>
        /// <returns></returns>
        public async Task NavigationUpAsync(int navigationId)
        {
            navigationId.CheckGreaterThan("navigationId", 0);
            var navigation = await _navigationRepository.GetAsync(navigationId);
            await NavigationUpAsync(navigation);
        }

        /// <summary>
        /// 菜单升序
        /// </summary>
        /// <param name="navigation">菜单</param>
        /// <returns></returns>
        public async Task NavigationUpAsync(Navigation navigation)
        {
            navigation.CheckNotNull("navigation");

            var preNavigation = _navigationRepository.GetAll().Where(p => p.OrderNo < navigation.OrderNo && p.ParentId == navigation.ParentId)
                .OrderByDescending(p => p.OrderNo).Take(1).FirstOrDefault();

            if (preNavigation != null)
            {
                int orderNo = navigation.OrderNo;
                navigation.OrderNo = preNavigation.OrderNo;
                preNavigation.OrderNo = orderNo;

                await _navigationRepository.UpdateAsync(navigation);
                await _navigationRepository.UpdateAsync(preNavigation);
            }
        }

        /// <summary>
        /// 菜单降序
        /// </summary>
        /// <param name="navigationId">菜单Id</param>
        /// <returns></returns>
        public async Task NavigationDownAsync(int navigationId)
        {
            navigationId.CheckGreaterThan("navigationId", 0);
            var navigation = await _navigationRepository.GetAsync(navigationId);
            await NavigationDownAsync(navigation);
        }

        /// <summary>
        /// 菜单降序
        /// </summary>
        /// <param name="navigation">菜单</param>
        /// <returns></returns>
        public async Task NavigationDownAsync(Navigation navigation)
        {
            navigation.CheckNotNull("navigation");
            var nextNavigation = _navigationRepository.GetAll().Where(p => p.OrderNo > navigation.OrderNo && p.ParentId == navigation.ParentId)
                .OrderBy(p => p.OrderNo).Take(1).FirstOrDefault();

            if (nextNavigation != null)
            {
                int orderNo = navigation.OrderNo;
                navigation.OrderNo = nextNavigation.OrderNo;
                nextNavigation.OrderNo = orderNo;

                await _navigationRepository.UpdateAsync(navigation);
                await _navigationRepository.UpdateAsync(nextNavigation);
            }
        }

        /// <summary>
        /// 获取用户导航菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<NavigationInfo>> GetUserNavigations()
        {
            Expression<Func<Navigation, bool>> exp = p => p.NavigationType == NavigationType.Anonymouse;
            if (AbpSession.UserId.HasValue)
            {
                exp.Or(p => p.NavigationType == NavigationType.Logined);

                var roleIds = _userRoleRepository.GetAll().Where(p => p.UserId == AbpSession.UserId.Value).Select(p => p.RoleId);
                var navigationIds = _roleNavigationRepository.GetAll().Where(p => roleIds.Contains(p.RoleId)).Select(p => p.NavigationId);
                exp.Or(p => p.NavigationType == NavigationType.RoleLimit && navigationIds.Contains(p.Id));
            }
            return await QueryNavigations(exp);
        }

        /// <summary>
        /// 获取全部菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<NavigationInfo>> GetAllNavigations()
        {
            return await QueryNavigations(p => true);
        }

        private async Task CheckNavigationAsync(Navigation navigation)
        {
            navigation.CheckNotNull("navigation");
            if (_navigationRepository.CheckExists(p => p.ParentId == navigation.ParentId && p.Name == navigation.Name, navigation.Id))
            {
                throw new UserFriendlyException("该菜单已存在");
            }

            if (navigation.ParentId != 0 && !_navigationRepository.CheckExists(p => p.Id == navigation.ParentId))
            {
                throw new UserFriendlyException("制定的父级菜单不存在");
            }
        }

        private async Task<ICollection<NavigationInfo>> QueryNavigations(Expression<Func<Navigation, bool>> query)
        {
            var navigations = await _navigationRepository.GetAll().AsNoTracking().Where(query).OrderBy(p => p.OrderNo).Select(p => new
            {
                p.Id,
                p.Name,
                p.Url,
                p.Remark,
                p.Icon,
                p.ParentId,
                p.NavigationType
            }).ToListAsync();

            Func<int, NavigationInfo[]> getChildren = null;
            getChildren = parentId =>
            {
                if (navigations.Any(m => m.ParentId == parentId))
                {
                    return navigations.Where(m => m.ParentId == parentId).Select(m => new NavigationInfo()
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
                return new NavigationInfo[] { };
            };

            return getChildren(0);
        }
    }
}
