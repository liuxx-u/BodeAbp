using Abp.Application.Services;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.Navigations.Dtos;
using System.Threading.Tasks;
using BodeAbp.Zero.Navigations.Domain;
using System.Collections.Generic;

namespace BodeAbp.Zero.Navigations
{
	/// <summary>
    ///  导航信息 服务
    /// </summary>
	[Description("导航信息接口")]
    public interface INavigationAppService : IApplicationService
    {
        #region 导航信息

        /// <summary>
        /// 获取 用户导航信息
        /// </summary>
        /// <returns></returns>
        Task<ICollection<NavigationInfo>> GetUserNavigations();
        
        /// <summary>
        /// 检查用户导航信息权限
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<bool> CheckUserNavigation(NavigationCheckInput input);
        
        /// <summary>
        /// 获取 用户导航信息
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <returns></returns>
        Task<ICollection<NavigationInfo>> GetRoleAndMenu(int roleid);

        /// <summary>
        /// 角色关联菜单
        /// </summary>
        /// <returns></returns>
        Task CreateRoleAndMenu(int roleid, string menuids);

        /// <summary>
        /// 获取 全部导航信息
        /// </summary>
        /// <returns></returns>
        Task<ICollection<NavigationInfo>> GetAllNavigations();

        /// <summary>
        /// 添加 导航信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateNavigation(NavigationInput input);

        /// <summary>
        /// 更新 导航信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateNavigation(NavigationInput input);
		

        /// <summary>
        /// 删除 导航信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteNavigation(IdInput input);


        /// <summary>
        /// 菜单升序
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task NavigationUp(IdInput input);

        /// <summary>
        /// 菜单降序
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task NavigationDown(IdInput input);

        #endregion
    }
}
