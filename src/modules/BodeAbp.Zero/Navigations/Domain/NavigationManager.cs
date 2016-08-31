using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.UI;

namespace BodeAbp.Zero.Navigations.Domain
{
    /// <summary>
    /// 导航菜单 领域服务
    /// </summary>
    public class NavigationManager : IDomainService
    {

        private readonly IRepository<Navigation> _navigationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="navigationRepository"></param>
        public NavigationManager(IRepository<Navigation> navigationRepository)
        {
            _navigationRepository = navigationRepository;
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
    }
}
