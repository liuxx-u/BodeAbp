using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BodeAbp.Zero.Application.Users.Dtos;
using BodeAbp.Zero.Users.Dtos;

namespace BodeAbp.Zero.Users
{
    public interface IUserAppService : IApplicationService
    {
        #region Account
        
        /// <summary>
        /// 获取 验证码
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task GetValidateCode(ValidateCodeInput input);

        /// <summary>
        /// 验证 验证码
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task ValidateCode(ValidateCodeInput input);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task CreateUser(CreateUserInput input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task ChangePassword(ChangePasswordInput input);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task ResetPassword(ResetPasswordInput input);

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        Task ChangeUserName(ChangeUserNameInput input);

        #endregion


        #region Admin

        /// <summary>
        /// 获取用户导航栏
        /// </summary>
        /// <returns>目录集合</returns>
        Task<IReadOnlyList<UserMenu>> GetUserNavigations();

        /// <summary>
        /// 获取 用户信息 分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<GetUserListOutput>> GetUserPagedList(QueryListPagedRequestInput input);

        /// <summary>
        /// 锁定  用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>业务操作结果</returns>
        Task ActiveUserOrNot(long userId);

        /// <summary>
        /// 获取 用户角色
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>角色数据</returns>
        IList<string> GetUserRoles(long? userId);

        /// <summary>
        /// 设置 用户角色
        /// </summary>
        /// <param name="roles">角色名集合</param>
        /// <returns>业务操作结果</returns>
        Task SetUserRoles(string roleNames);

        #endregion

    }
}
