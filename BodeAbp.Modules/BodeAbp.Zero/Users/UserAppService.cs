using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Application.Services.Query;
using Abp.Authorization;
using Abp.Authorization.Users.Domain;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Utils.Drawing;
using BodeAbp.Plugins.Core;
using BodeAbp.Zero.Application.Users.Dtos;
using BodeAbp.Zero.Roles.Domain;
using BodeAbp.Zero.Users.Domain;
using BodeAbp.Zero.Users.Dtos;
using Microsoft.AspNet.Identity;
using System.Linq;
using Abp.Extensions;

namespace BodeAbp.Zero.Users
{
    public class UserAppService : ApplicationService,IUserAppService
    {
        public ISms Sms { protected get; set; }
        public IEmailSender EmailSender { protected get; set; }
        public AbpUserManager UserManager { protected get; set; }
        public IRepository<User, long> UserRepo { protected get; set; }
        public IRepository<Role, int> RoleRepo { protected get; set; }
        public IUserNavigationManager UserNavigationManager { protected get; set; }
        public IRepository<ValidateCode, long> ValidateCodeRepo { protected get; set; }

        #region Account
        
        /// <summary>
        /// 获取 验证码
        /// </summary>
        /// <param name="input">验证码Input</param>
        public async Task GetValidateCode(ValidateCodeInput input)
        {
            if (input.CodeType == CodeType.注册)
            {
                if (UserRepo.CheckExists(p => p.UserName == input.CodeKey))
                {
                    throw new UserFriendlyException("手机号已注册，不能获取注册验证码.");
                }
            }
            if (input.CodeType == CodeType.找回密码 || input.CodeType == CodeType.动态登录)
            {
                if (!UserRepo.CheckExists(p => p.UserName == input.CodeKey))
                {
                    throw new UserFriendlyException("该用户未注册，不能获取验证码.");
                }
            }

            if (input.ValidateType == ValidateType.手机)
            {
                await SendValidateCode(input, code =>
                {
                    var smsContent = "您本次的验证码为" + code + "，工作人员不会向您索要此验证码，请勿向任何人泄露。[不同]";
                    //Sms.Send(phoneNo, 1, smsContent);
                });
            }
            else
            {
                await SendValidateCode(input, code =>
                {
                    string subject = "验证码";
                    string body = "您本次的验证码为" + code + "，工作人员不会向您索要此验证码，请勿向任何人泄露。[不同]";
                    EmailSender.Send(input.CodeKey, subject, body);
                });
            }
        }

        /// <summary>
        /// 验证 验证码
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        public async Task ValidateCode(ValidateCodeInput input)
        {
            //有效期30分钟
            DateTime ValidityPeriod = DateTime.Now.AddMinutes(-30);

            var code = await ValidateCodeRepo.FirstOrDefaultAsync(
                p => p.CodeKey == input.CodeKey
                && p.CodeType == input.CodeType
                && p.ValidateType == input.ValidateType
                && p.Code == input.Code
                && p.CreationTime > ValidityPeriod);

            if (code == null)
            {
                throw new UserFriendlyException("验证失败");
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        public async Task CreateUser(CreateUserInput input)
        {
            await ValidateCode(input.ValidateCode);

            var user = input.MapTo<User>();
            user.IsActive = true;
            user.Password = new PasswordHasher().HashPassword(input.Password);
            if (input.ValidateCode.ValidateType == ValidateType.手机)
            {
                user.IsPhoneNoConfirm = true;
            }
            else
            {
                user.IsEmailConfirmed = true;
            }
            var identityResult = await UserManager.CreateAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new UserFriendlyException(identityResult.Errors.JoinAsString(" "));
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task ChangePassword(ChangePasswordInput input)
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.Value);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }

            if (!await UserManager.CheckPasswordAsync(user, input.Password))
            {
                throw new UserFriendlyException("原密码错误");
            }
            UserManager.RemovePassword(user.Id);
            UserManager.AddPassword(user.Id, input.NewPassword);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task ResetPassword(ResetPasswordInput input)
        {
            await ValidateCode(input.ValidateCode);
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.Value);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            UserManager.RemovePassword(user.Id);
            UserManager.AddPassword(user.Id, input.NewPassword);
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task ChangeUserName(ChangeUserNameInput input)
        {
            await ValidateCode(input.ValidateCode);
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (!await UserManager.CheckPasswordAsync(user, input.Password))
            {
                throw new UserFriendlyException("密码错误");
            }
            if (UserRepo.CheckExists(p => p.UserName == input.UserName, AbpSession.UserId.Value))
            {
                throw new UserFriendlyException("该用户名已存在");
            }
            user.UserName = input.UserName;
            await UserRepo.UpdateAsync(user);
        }


        #endregion

        #region Admin
        
        /// <summary>
        /// 获取用户导航栏
        /// </summary>
        /// <returns>目录集合</returns>
        public async Task<IReadOnlyList<UserMenu>> GetUserNavigations()
        {
            var userMenus = await UserNavigationManager.GetMenusAsync(AbpSession.ToUserIdentifier());
            return userMenus;
        }

        /// <summary>
        /// 获取用户列表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns>列表数据</returns>
        public async Task<PagedResultOutput<GetUserListOutput>> GetUserPagedList(QueryListPagedRequestInput input)
        {
            int total;
            var list = await UserRepo.GetAll().Where(input, out total).ToListAsync();
            return new PagedResultOutput<GetUserListOutput>(total, list.MapTo<List<GetUserListOutput>>());
        }

        /// <summary>
        /// 激活/锁定  用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>业务操作结果</returns>
        public async Task ActiveUserOrNot(long userId)
        {
            var user = await UserManager.GetUserByIdAsync(userId);
            user.IsActive = !user.IsActive;
            await UserRepo.UpdateAsync(user);
        }

        /// <summary>
        /// 获取 用户角色
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>角色数据</returns>
        public IList<string> GetUserRoles(long? userId)
        {
            var roles = UserManager.GetRoles(userId.Value);
            return roles;
        }

        /// <summary>
        /// 设置 用户角色
        /// </summary>
        /// <param name="roles">角色名集合</param>
        /// <returns>业务操作结果</returns>
        [AbpAuthorize]
        public async Task SetUserRoles(string roleNames)
        {
            var roleArr = roleNames.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);
            await UserManager.SetRoles(user, roleArr);
        }

        #endregion



        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="input">验证码Input</param>
        /// <returns></returns>
        private async Task SendValidateCode(ValidateCodeInput input, Action<string> sendAction)
        {
            string validateCode = new ValidateCoder().GetCode(6, ValidateCodeType.Number);
            var codeEntity = input.MapTo<ValidateCode>();
            codeEntity.Code = validateCode;
            await ValidateCodeRepo.InsertAsync(codeEntity);
            sendAction(validateCode);
        }


        
    }
}
