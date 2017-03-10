using System;
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
using Abp.Runtime.Caching;
using BodeAbp.Zero.Permissions.Domain;

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

        private readonly ICacheManager _cacheManager;

        private readonly IRepository<RolePermission, long> _rolePermissionRepository;
        
        private readonly string _userNavigationCacheKey = "BodeAbp.Zero.UserNavigations";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="navigationRepository"></param>
        /// <param name="roleNavigationRepository"></param>
        /// <param name="userRoleRepository"></param>
        /// <param name="cacheManager"></param>
        public NavigationManager(IRepository<Navigation> navigationRepository
            , IRepository<RoleNavigation> roleNavigationRepository
            , IRepository<UserRole, long> userRoleRepository
            , ICacheManager cacheManager
            , IRepository<RolePermission, long> rolePermissionRepository)
        {
            _navigationRepository = navigationRepository;
            _userRoleRepository = userRoleRepository;
            _roleNavigationRepository = roleNavigationRepository;

            _cacheManager = cacheManager;
            _rolePermissionRepository = rolePermissionRepository;
        }

        /// <summary>
        /// 创建  菜单
        /// </summary>
        /// <param name="navigation">菜单</param>
        /// <returns></returns>
        public async Task CreateNavigationAsync(Navigation navigation)
        {
            CheckNavigationAsync(navigation);
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
            CheckNavigationAsync(navigation);
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
                exp = exp.Or(p => p.NavigationType == NavigationType.Logined);
                var roleIds = _userRoleRepository.GetAll().Where(p => p.UserId == AbpSession.UserId.Value).Select(p => p.RoleId).ToList();

                var navigationIds = _roleNavigationRepository.GetAll().Where(p => roleIds.Contains(p.RoleId)).Select(p => p.NavigationId).ToList();
                exp = exp.Or(p => p.NavigationType == NavigationType.RoleLimit && navigationIds.Contains(p.Id));
            }

            return await QueryNavigationTrees(exp);
        }
        /// <summary>
        /// (改)(新)获取用户导航菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<NavigationInfo>> GetUserRoleNavigation()
        {
            Expression<Func<Navigation, bool>> exp = p => p.NavigationType == NavigationType.Anonymouse;
            if (AbpSession.UserId.HasValue)
            {
                exp = exp.Or(p => p.NavigationType == NavigationType.Logined);
                var roleIds = _userRoleRepository.GetAll().Where(p => p.UserId == AbpSession.UserId.Value).Select(p => p.RoleId).ToList();
                var permissionNames = await _rolePermissionRepository.GetAll().Where(n => roleIds.Contains(n.RoleId)).Select(n => n.Name).Distinct().ToListAsync();
                exp= exp.Or(n =>n.NavigationType == NavigationType.RoleLimit && permissionNames.Contains(n.RequiredPermissionName));
            }

            return await QueryNavigationTrees(exp);
        }

        /// <summary>
        /// 获取全部菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<NavigationInfo>> GetAllNavigations()
        {
            return await QueryNavigationTrees(p => true);
        }

        /// <summary>
        /// 检查用户是否拥有权限
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public async Task<bool> CheckUserNavigation(string url)
        {
            var navigation = (await GetAllNavigationInfos()).FirstOrDefault(p => CompareNavigationUrl(p, url));

            if (navigation == null || navigation.NavigationType == NavigationType.Anonymouse) return true;
            if (navigation.NavigationType == NavigationType.Logined && AbpSession.UserId.HasValue) return true;

            if (navigation.NavigationType == NavigationType.RoleLimit)
            {
                if (!AbpSession.UserId.HasValue) return false;
                return (await GetUserNavigationInfos()).Any(p => CompareNavigationUrl(p, url));
            }
            return false;
        }

        /// <summary>
        /// 获取角色关联的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<NavigationInfo>> GetRoleAndMenu(int roleid)
        {
            var navigations = await _navigationRepository.GetAll().AsNoTracking().OrderBy(p => p.OrderNo).Select(p => new
            {
                p.Id,
                p.Name,
                p.Url,
                p.Remark,
                p.Icon,
                p.ParentId,
                p.NavigationType
            }).ToListAsync();

            var roleNavigationList = await _roleNavigationRepository.GetAll().Where(n => n.RoleId == roleid).ToListAsync();


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
                        Children = getChildren(m.Id),
                        IsRelation = roleNavigationList.Count(n => n.NavigationId == m.Id) > 0
                    }).ToArray();
                }
                return new NavigationInfo[] { };
            };

            return getChildren(0);
        }

        /// <summary>
        /// 角色关联菜单
        /// </summary>
        /// <returns></returns>
        public async Task CreateRoleAndMenu(int roleid, string menuids)
        {
            var menidlist = menuids.Split(',').Select(n => new RoleNavigation
            {
                CreationTime = DateTime.Now,
                CreatorUserId = AbpSession.UserId,
                NavigationId = int.Parse(n),
                RoleId = roleid
            }).ToList();
            var frontList = await _roleNavigationRepository.GetAll().Where(n => n.RoleId == roleid).ToListAsync();
            var noMaplist = new List<RoleNavigation>(); //共同拥有的
            var addMaplist = menidlist;       //最后新增的数据
            var delMaplist = frontList;               //待删除数据
            if (frontList.Count > 0)
            {
                //取出共同拥有的数据
                foreach (var couponProductMap in menidlist)
                {
                    noMaplist.AddRange(frontList.Where(productMap => couponProductMap.RoleId == productMap.RoleId && productMap.NavigationId == couponProductMap.NavigationId));
                }
                //新增的数据移除共同拥有的数据
                foreach (var productMap in noMaplist)
                {
                    addMaplist.RemoveAll(n => n.RoleId == productMap.RoleId && n.NavigationId == productMap.NavigationId);
                }
                //删除的数据移除共同拥有的数据
                foreach (var productMap in noMaplist)
                {
                    delMaplist.RemoveAll(n => n.RoleId == productMap.RoleId && n.NavigationId == productMap.NavigationId);
                }
            }
            //新增新数据
            foreach (var map in addMaplist)
            {
                await _roleNavigationRepository.InsertAsync(map);
            }
            //删除旧数据
            foreach (var map in delMaplist)
            {
                await _roleNavigationRepository.DeleteAsync(map);
            }

            var userIds = await _userRoleRepository.GetAll().AsNoTracking()
                .Where(p => p.RoleId == roleid).Select(p => p.UserId)
                .ToArrayAsync();

            //删除缓存
            var cacher = _cacheManager.GetCache(_userNavigationCacheKey).AsTyped<long, ICollection<NavigationInfo>>();
            foreach (var userId in userIds)
            {
                cacher.Remove(userId);
            }
        }

        #region 私有方法

        private void CheckNavigationAsync(Navigation navigation)
        {
            navigation.CheckNotNull("navigation");
            if (_navigationRepository.CheckExists(p => p.ParentId == navigation.ParentId && p.Name == navigation.Name, navigation.Id))
            {
                throw new UserFriendlyException("该菜单已存在");
            }

            if (navigation.ParentId != 0 && !_navigationRepository.CheckExists(p => p.Id == navigation.ParentId))
            {
                throw new UserFriendlyException("指定的父级菜单不存在");
            }
        }

        /// <summary>
        /// 获取用户导航信息集合
        /// </summary>
        /// <returns></returns>
        private async Task<ICollection<NavigationInfo>> GetUserNavigationInfos()
        {
            var cacheKey = AbpSession.UserId.HasValue ? AbpSession.UserId.Value : 0;

            return await _cacheManager
                .GetCache(_userNavigationCacheKey)
                .GetAsync(cacheKey, () =>
                {
                    Expression<Func<Navigation, bool>> exp = p => p.NavigationType == NavigationType.Anonymouse;
                    if (AbpSession.UserId.HasValue)
                    {
                        exp = exp.Or(p => p.NavigationType == NavigationType.Logined);
                        var roleIds = _userRoleRepository.GetAll().Where(p => p.UserId == AbpSession.UserId.Value).Select(p => p.RoleId).ToList();
                        var navigationIds = _roleNavigationRepository.GetAll().Where(p => roleIds.Contains(p.RoleId)).Select(p => p.NavigationId).ToList();
                        exp = exp.Or(p => p.NavigationType == NavigationType.RoleLimit && navigationIds.Contains(p.Id));
                    }
                    return QueryNavigations(exp);
                });
        }

        /// <summary>
        /// 获取所有导航信息集合
        /// </summary>
        /// <returns></returns>
        private async Task<ICollection<NavigationInfo>> GetAllNavigationInfos()
        {
            return await _cacheManager
                .GetCache(_userNavigationCacheKey)
                .GetAsync("all", () => QueryNavigations(p => true));
        }

        /// <summary>
        /// 根据条件查询导航信息集合
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        private async Task<ICollection<NavigationInfo>> QueryNavigations(Expression<Func<Navigation, bool>> query)
        {
            var navigations = await _navigationRepository.GetAll().AsNoTracking().Where(query).OrderBy(p => p.OrderNo).Select(p => new NavigationInfo
            {
                Id = p.Id,
                Name = p.Name,
                Url = p.Url,
                Remark = p.Remark,
                Icon = p.Icon,
                ParentId = p.ParentId,
                NavigationType = p.NavigationType,
                RequiredPermissionName = p.RequiredPermissionName
            }).ToListAsync();

            return navigations;
        }

        /// <summary>
        /// 根据条件查询导航信息集合（树形，递归）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        private async Task<ICollection<NavigationInfo>> QueryNavigationTrees(Expression<Func<Navigation, bool>> query)
        {
            var navigations = await QueryNavigations(query);

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
                        Children = getChildren(m.Id),
                        RequiredPermissionName=m.RequiredPermissionName
                    }).ToArray();
                }
                return new NavigationInfo[] { };
            };

            return getChildren(0);
        }

        /// <summary>
        /// 比较导航信息与url是否匹配
        /// </summary>
        /// <param name="navigation">导航信息</param>
        /// <param name="url">url</param>
        /// <returns></returns>
        private bool CompareNavigationUrl(NavigationInfo navigation, string url)
        {
            url = url.ToLower().Trim().TrimEnd(new[] { '#' });
            var navigationUrl = navigation.Url.ToLower().Trim().TrimEnd(new[] { '#' });

            var urlIndex = url.IndexOf("?");
            if (urlIndex <= 0) return url == navigationUrl;

            var start = url.Substring(0, urlIndex);
            if (!navigationUrl.StartsWith(start)) return false;

            Func<string, Dictionary<string, string>> getParamFunc = p =>
            {
                var paramDic = new Dictionary<string, string>();
                int paramIndex = p.IndexOf("?");
                if (paramIndex <= 0) return paramDic;


                string param = p.Substring(paramIndex + 1);
                var keyValues = param.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in keyValues)
                {
                    var arr = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim()).ToArray();
                    if (arr.Count() != 2) continue;
                    paramDic.Add(arr[0], arr[1]);
                }
                return paramDic;
            };

            var urlParams = getParamFunc(url);
            var navigationParams = getParamFunc(navigationUrl);
            foreach (var item in navigationParams)
            {
                if (!urlParams.ContainsKey(item.Key) || urlParams[item.Key] != item.Value) return false;
            }
            return true;
        }

        #endregion

    }
}
