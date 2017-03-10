using Abp.Auditing;
using Abp.Authorization.Users.Domain;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using BodeAbp.Zero.Roles.Domain;
using BodeAbp.Zero.Users.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebDemo.Web.Areas.Admin.ViewModels.Account;
using static Abp.Authorization.Users.Domain.AbpUserManager;

namespace WebDemo.Web.Areas.Admin.Controllers
{
    public class AccountController : AdminControllerBase
    {
        private readonly AbpUserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        // GET: Admin/Account
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(
            AbpUserManager userManager,
            RoleManager roleManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #region Login / Logout

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            return View(new LoginFormViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            CheckModelState();

            var loginResult = await GetLoginResultAsync(loginModel.UserName, loginModel.Password);

            await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/Admin/Home/Index";
            }

            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            return Json(new AjaxResponse { TargetUrl = returnUrl });
        }

        private async Task<AbpLoginResult> GetLoginResultAsync(string username, string password)
        {
            var loginResult = await _userManager.LoginAsync(username, password);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, username);
            }
        }

        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserName:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                //case AbpLoginResultType.InvalidTenancyName:
                //    return new UserFriendlyException(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                //case AbpLoginResultType.TenantIsNotActive:
                //    return new UserFriendlyException(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case AbpLoginResultType.EmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "Your email address is not confirmed. You can not login"); //TODO: localize message
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        #endregion
    }
}